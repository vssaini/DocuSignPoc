using System.Threading.Tasks;
using DocuSignPoc.Models.DocuSign;

namespace DocuSignPoc.Contracts
{
    public interface IDocuSignService
    {
        bool IsConsentGranted();
        string GetConsentUrl();
        Task SendEmailAsync(DocuSignEnvelope dsEnvelope);

        /// <summary>
        /// Validate DocuSign request.
        /// </summary>
        void ValidateRequest(string payload, string signature);

        string SaveSignedFile(string payload);
    }
}
