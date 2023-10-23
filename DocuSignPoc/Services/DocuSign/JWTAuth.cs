using System.Collections.Generic;
using System.IO;
using DocuSign.eSign.Client;
using DocuSign.eSign.Client.Auth;

namespace DocuSignPoc.Services.DocuSign
{
    public static class JwtAuth
    {
        /// <summary>
        /// Uses Json Web Token (JWT) Authentication Method to obtain the necessary information needed to make API calls.
        /// </summary>
        /// <returns>Auth token needed for API calls</returns>
        public static OAuth.OAuthToken Authenticate(string clientId, string impersonatedUserId, string authServer, string privateKeyFilePath)
        {
            var apiClient = new DocuSignClient();

            var scopes = new List<string>
                {
                    "signature",
                    "impersonation",
                };

            return apiClient.RequestJWTUserToken(
                clientId,
                impersonatedUserId,
                authServer,
                ReadFileContent(privateKeyFilePath),
                1,
                scopes);
        }

        internal static byte[] ReadFileContent(string path)
        {
            return File.ReadAllBytes(path);
        }
    }
}