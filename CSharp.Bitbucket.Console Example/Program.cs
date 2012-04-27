using System;
using System.Diagnostics;
using CSharp.Bitbucket.Api;
using CSharp.Bitbucket.Connect;
using Spring.Json;
using Spring.Social.OAuth1;

namespace CSharp.Bitbucket.Console_Example
{
	class Program
	{
		// Register your own Bitbucket app at http://confluence.atlassian.com/display/BITBUCKET/Using+the+bitbucket+REST+APIs

		// Set your consumer key & secret here
		private const string BitbucketApiKey = "ENTER YOUR KEY HERE";
		private const string BitbucketApiSecret = "ENTER YOUR SECRET HERE";

		// Set your username to request
		private const string Username = "scottksmith95";

		private static void Main(string[] args)
		{
			try
			{
				var bitbucketServiceProvider = new BitbucketServiceProvider(BitbucketApiKey, BitbucketApiSecret);

				/* OAuth 'dance' */

				// Authentication using Out-of-band/PIN Code Authentication
				Console.Write("Getting request token...");
				var oauthToken = bitbucketServiceProvider.OAuthOperations.FetchRequestTokenAsync("oob", null).Result;
				Console.WriteLine("Done");

				var authenticateUrl = bitbucketServiceProvider.OAuthOperations.BuildAuthorizeUrl(oauthToken.Value, null);
				Console.WriteLine("Redirect user for authentication: " + authenticateUrl);
				Process.Start(authenticateUrl);
				Console.WriteLine("Enter PIN Code from Bitbucket authorization page:");
				var pinCode = Console.ReadLine();

				Console.Write("Getting access token...");
				var requestToken = new AuthorizedRequestToken(oauthToken, pinCode);
				var oauthAccessToken = bitbucketServiceProvider.OAuthOperations.ExchangeForAccessTokenAsync(requestToken, null).Result;
				Console.WriteLine("Done");

				/* API */

				var bitbucket = bitbucketServiceProvider.GetApi(oauthAccessToken.Value, oauthAccessToken.Secret);

				bitbucket.RestOperations.GetForObjectAsync<JsonValue>("https://api.bitbucket.org/1.0/users/" + Username)
					.ContinueWith(task => Console.WriteLine("Result: " + task.Result.ToString()));
			}
			catch (AggregateException ae)
			{
				ae.Handle(ex =>
				{
				    if (ex is BitbucketApiException)
				    {
				        Console.WriteLine(ex.Message);
				        return true;
				    }
				    return false;
				});
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
			finally
			{
				Console.WriteLine("--- hit <return> to quit ---");
				Console.ReadLine();
			}
		}
	}
}