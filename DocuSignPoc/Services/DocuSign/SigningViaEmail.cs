using DocuSign.eSign.Api;
using DocuSign.eSign.Client;
using DocuSign.eSign.Client.Auth;
using DocuSign.eSign.Model;
using DocuSignPoc.Models.DocuSign;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using EnvelopeSummary = DocuSign.eSign.Model.EnvelopeSummary;

namespace DocuSignPoc.Services.DocuSign
{
    internal class SigningViaEmail
    {
        /// <summary>
        /// Creates an envelope that would include PDF document and add a signer and cc recipients to be notified via email.
        /// </summary>
        /// <param name="dsEnvelope">Holding name and email address of signer as well as file.</param>
        /// <param name="oAuthToken">OAuth Token for API call</param>
        /// <returns>EnvelopeId for the new envelope</returns>
        public static async Task<string> CreateEnvelopeAsync(DocuSignEnvelope dsEnvelope, OAuth.OAuthToken oAuthToken)
        {
            if (oAuthToken == null)
                throw new NullReferenceException($"{nameof(oAuthToken)} is null");

            // STEP 1/3 - Make Envelope
            EnvelopeDefinition envDefinition = MakeEnvelope(dsEnvelope);

            var dsClient = GetDocuSignClient(oAuthToken.access_token);
            var accountId = GetAccountId(dsClient, oAuthToken.access_token);

            EnvelopesApi envelopesApi = new EnvelopesApi(dsClient);
            EnvelopeSummary results = await envelopesApi.CreateEnvelopeAsync(accountId, envDefinition);
            return results.EnvelopeId;
        }

        private static EnvelopeDefinition MakeEnvelope(DocuSignEnvelope dsEnvelope)
        {
            // STEP 3/3
            var env = new EnvelopeDefinition
            {
                EmailSubject = "Please sign the document",
                Documents = GetDocument(dsEnvelope),
                Recipients = GetRecipients(dsEnvelope)
            };

            // To request that the envelope be created as a draft, set to "created"
            // Request that the envelope be sent by setting |status| to "sent".
            env.Status = "sent";

            return env;
        }

        private static Recipients GetRecipients(DocuSignEnvelope dsEnvelope)
        {
            // create a signer recipient to sign the document, identified by name and email
            // We're setting the parameters via the object creation
            var signer = new Signer
            {
                Email = dsEnvelope.SignerEmail,
                Name = dsEnvelope.SignerName,
                RecipientId = "1",
                RoutingOrder = "1",
                Tabs = GetTabs()
            };

            // routingOrder (lower means earlier) determines the order of deliveries
            // to the recipients. Parallel routing order is supported by using the
            // same integer as the order for two or more recipients.

            // create a cc recipient to receive a copy of the documents, identified by name and email
            // We're setting the parameters via setters

            var ccEmail = ConfigurationManager.AppSettings["DSCarbonCopyEmail"];
            var ccName = ConfigurationManager.AppSettings["DSCarbonCopyName"];

            var cc = new CarbonCopy
            {
                Email = ccEmail,
                Name = ccName,
                RecipientId = "2",
                RoutingOrder = "2",
            };

            // The envelope has two recipients.
            // recipient 1 - signer
            // recipient 2 - cc
            // The envelope will be sent first to the signer.
            // After it is signed, a copy is sent to the cc person.
            var recipients = new Recipients
            {
                Signers = new List<Signer> { signer },
                CarbonCopies = new List<CarbonCopy> { cc },
            };

            return recipients;
        }

        private static Tabs GetTabs()
        {
            // Create signHere fields (also known as tabs) on the documents,
            // We're using anchor (autoPlace) positioning
            //
            // The DocuSign platform searches throughout your envelope's
            // documents for matching anchor strings. So the
            // signHere tab will be used in document since they
            // use the same anchor string for their "signer 1" tabs.
            var signHere = new SignHere
            {
                AnchorString = "Please sign here",
                AnchorUnits = "pixels",
                AnchorYOffset = "-23",
                AnchorXOffset = "20",
            };

            // Tabs are set per recipient / signer
            return new Tabs
            {
                SignHereTabs = new List<SignHere> { signHere }
            };
        }

        private static List<Document> GetDocument(DocuSignEnvelope dsEnvelope)
        {
            // STEP 2/3 - Create PDF bytes   
            var base64Pdf = Convert.ToBase64String(dsEnvelope.FileBytes);

            var pdfDoc = new Document
            {
                DocumentBase64 = base64Pdf,
                Name = dsEnvelope.FileNameWithExtension, // can be different from actual file name
                FileExtension = "pdf",
                DocumentId = "1" // a label used to reference the doc
            };

            // The order in the docs array determines the order in the envelope
            return new List<Document> { pdfDoc };
        }

        private static DocuSignClient GetDocuSignClient(string accessToken)
        {
            var baseUri = ConfigurationManager.AppSettings["DSBaseUri"];

            var docuSignClient = new DocuSignClient(baseUri);
            docuSignClient.Configuration.DefaultHeader.Add("Authorization", "Bearer " + accessToken);

            return docuSignClient;
        }

        private static string GetAccountId(DocuSignClient dsClient, string accessToken)
        {
            var userInfo = dsClient.GetUserInfo(accessToken);
            var account = userInfo.Accounts.Single();

            return account.AccountId;
        }
    }
}