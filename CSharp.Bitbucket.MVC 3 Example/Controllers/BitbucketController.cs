using System.Web.Mvc;
using CSharp.Bitbucket.Api.Interfaces;
using CSharp.Bitbucket.Connect;
using Spring.Json;
using Spring.Social.OAuth1;

namespace CSharp.Bitbucket.MVC_3_Example.Controllers
{
    public class BitbucketController : Controller
    {
		// Register your own Bitbucket app at http://confluence.atlassian.com/display/BITBUCKET/Using+the+bitbucket+REST+APIs

		// Configure the Callback URL
		private const string CallbackUrl = "http://localhost:14915/Bitbucket/Callback";

		// Set your consumer key & secret here
		private const string BitbucketApiKey = "ENTER YOUR KEY HERE";
		private const string BitbucketApiSecret = "ENTER YOUR SECRET HERE";

		// Set your username to request
    	private const string Username = "scottksmith95";

		readonly IOAuth1ServiceProvider<IBitbucket> _bitbucketProvider = new BitbucketServiceProvider(BitbucketApiKey, BitbucketApiSecret);

		public ActionResult Index()
		{
			var token = Session["AccessToken"] as OAuthToken;
			if (token != null)
			{
				var bitbucketClient = _bitbucketProvider.GetApi(token.Value, token.Secret);
				var result = bitbucketClient.RestOperations.GetForObjectAsync<JsonValue>("https://api.bitbucket.org/1.0/users/" + Username).Result;

				ViewBag.TokenValue = token.Value;
				ViewBag.TokenSecret = token.Secret;
				ViewBag.ResultText = result.ToString();

				return View();
			}

			var requestToken = _bitbucketProvider.OAuthOperations.FetchRequestTokenAsync(CallbackUrl, null).Result;

			Session["RequestToken"] = requestToken;

			return Redirect(_bitbucketProvider.OAuthOperations.BuildAuthenticateUrl(requestToken.Value, null));
		}

		public ActionResult Callback(string oauth_verifier)
		{
			var requestToken = Session["RequestToken"] as OAuthToken;
			var authorizedRequestToken = new AuthorizedRequestToken(requestToken, oauth_verifier);
			var token = _bitbucketProvider.OAuthOperations.ExchangeForAccessTokenAsync(authorizedRequestToken, null).Result;

			Session["AccessToken"] = token;

			return RedirectToAction("Index");
		}
    }
}
