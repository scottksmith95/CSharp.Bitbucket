#region License

/*
 * Copyright 2002-2012 the original author or authors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#endregion

namespace CSharp.Bitbucket.Api
{
    /// <summary>
    /// The <see cref="BitbucketApiError"/> enumeration is used by the <see cref="BitbucketApiException"/> class 
    /// to indicate what kind of error caused the exception.
    /// </summary>
	/// <author>Scott Smith</author>
    public enum BitbucketApiError
    {
		/// <summary>
		/// 400 status codes.
		/// </summary>
		ClientError,

        /// <summary>
		/// 500 status codes.
        /// </summary>
        ServerError
    }
}
