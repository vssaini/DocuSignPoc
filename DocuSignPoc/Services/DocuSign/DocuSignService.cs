using DocuSign.eSign.Client;
using DocuSignPoc.Contracts;
using DocuSignPoc.Models.DocuSign;
using DocuSignPoc.Models.Exceptions;
using Newtonsoft.Json;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static DocuSign.eSign.Client.Auth.OAuth;
using DocuSignResponse = DocuSignPoc.Models.DocuSign.DocuSignResponse;

namespace DocuSignPoc.Services.DocuSign
{
    public class DocuSignService : IDocuSignService
    {
        public bool IsConsentGranted()
        {
            var oAuthToken = GetOAuthToken();
            return oAuthToken != null;
        }

        private OAuthToken GetOAuthToken(bool throwException = false)
        {
            OAuthToken oAuthToken;
            try
            {
                var authServer = ConfigurationManager.AppSettings["DSAuthServer"];
                var clientId = ConfigurationManager.AppSettings["DSClientId"];
                var impersonatedUserId = ConfigurationManager.AppSettings["DSImpersonatedUserID"];
                var keyFilePath = GetPrivateKeyFilePath();

                oAuthToken = JwtAuth.Authenticate(clientId, impersonatedUserId, authServer, keyFilePath);
            }
            catch (ApiException)
            {
                if (throwException)
                    throw;

                return null;
            }

            return oAuthToken;
        }

        private static string GetPrivateKeyFilePath()
        {
            var privateKeyFileName = ConfigurationManager.AppSettings["DSPrivateKeyFileName"];
            var keyFilePath = HttpContext.Current.Server.MapPath($"~/App_Data/{privateKeyFileName}");

            return keyFilePath;
        }

        public string GetConsentUrl()
        {
            // build a URL to provide consent for this Integration Key (aka ClientId) and this userId
            var authServer = ConfigurationManager.AppSettings["DSAuthServer"];
            var clientId = ConfigurationManager.AppSettings["DSClientId"];
            var redirectUri = ConfigurationManager.AppSettings["DSRedirectUri"];

            var url = $"https://{authServer}/oauth/auth?response_type=code&scope=impersonation%20signature&client_id={clientId}&redirect_uri={redirectUri}";

            return url;
        }

        public async Task SendEmailAsync(DocuSignEnvelope dsEnvelope)
        {
            var oAuthToken = GetOAuthToken(true);
            var envelopeId = await SigningViaEmail.CreateEnvelopeAsync(dsEnvelope, oAuthToken);

            await SaveEnvelopeIdForQuoteFileAsync(envelopeId, dsEnvelope.FileId);
        }

        private async Task SaveEnvelopeIdForQuoteFileAsync(string envelopeId, int fileId)
        {
            // TODO: Save envelopeId and fileId in database
        }

        public void ValidateRequest(string payload, string signature)
        {
            var secret = ConfigurationManager.AppSettings["DSSecret"];

            if (string.IsNullOrWhiteSpace(signature) || !HMACValidation.HashIsValid(secret, payload, signature))
                throw new BadRequestException("Webhook signature is not valid.");
        }

        public string SaveSignedFile(string payload)
        {
            var dsResponse = JsonConvert.DeserializeObject<DocuSignResponse>(payload);
            if (dsResponse == null)
                throw new BadRequestException("Response from DocuSign is null");

            var envelopeId = dsResponse.data.envelopeId;
            var envelopeDocument = dsResponse.data.envelopeSummary.envelopeDocuments
                .First(x => x.documentId == "1");

            var pdfBytes = envelopeDocument.PDFBytes;
            var blobName = $"Signed_{envelopeDocument.name}";

            return "Implementation pending";
        }
    }
}