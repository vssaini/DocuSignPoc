using System;
using System.Collections.Generic;

namespace DocuSignPoc.Models.DocuSign
{
	public class Data
	{
		public string accountId { get; set; }
		public string userId { get; set; }
		public string envelopeId { get; set; }
		public EnvelopeSummary envelopeSummary { get; set; }
	}

	public class EnvelopeDocument
	{
		public string documentId { get; set; }
		public string documentIdGuid { get; set; }
		public string name { get; set; }
		public string type { get; set; }
		public string uri { get; set; }
		public string order { get; set; }
		public List<Page> pages { get; set; }
		public string display { get; set; }
		public string includeInDownload { get; set; }
		public string signerMustAcknowledge { get; set; }
		public string templateRequired { get; set; }
		public string authoritativeCopy { get; set; }
		public byte[] PDFBytes { get; set; }
	}

	public class EnvelopeMetadata
	{
		public string allowAdvancedCorrect { get; set; }
		public string enableSignWithNotary { get; set; }
		public string allowCorrect { get; set; }
	}

	public class EnvelopeSummary
	{
		public string status { get; set; }
		public string documentsUri { get; set; }
		public string recipientsUri { get; set; }
		public string attachmentsUri { get; set; }
		public string envelopeUri { get; set; }
		public string emailSubject { get; set; }
		public string envelopeId { get; set; }
		public string signingLocation { get; set; }
		public string customFieldsUri { get; set; }
		public string notificationUri { get; set; }
		public string enableWetSign { get; set; }
		public string allowMarkup { get; set; }
		public string allowReassign { get; set; }
		public DateTime createdDateTime { get; set; }
		public DateTime lastModifiedDateTime { get; set; }
		public DateTime deliveredDateTime { get; set; }
		public DateTime initialSentDateTime { get; set; }
		public DateTime sentDateTime { get; set; }
		public DateTime completedDateTime { get; set; }
		public DateTime statusChangedDateTime { get; set; }
		public string documentsCombinedUri { get; set; }
		public string certificateUri { get; set; }
		public string templatesUri { get; set; }
		public string expireEnabled { get; set; }
		public DateTime expireDateTime { get; set; }
		public string expireAfter { get; set; }
		public Sender sender { get; set; }
		public List<Folder> folders { get; set; }
		public List<EnvelopeDocument> envelopeDocuments { get; set; }
		public string purgeState { get; set; }
		public string envelopeIdStamping { get; set; }
		public string is21CFRPart11 { get; set; }
		public string signerCanSignOnMobile { get; set; }
		public string autoNavigation { get; set; }
		public string isSignatureProviderEnvelope { get; set; }
		public string hasFormDataChanged { get; set; }
		public string allowComments { get; set; }
		public string hasComments { get; set; }
		public string allowViewHistory { get; set; }
		public EnvelopeMetadata envelopeMetadata { get; set; }
		public object anySigner { get; set; }
		public string envelopeLocation { get; set; }
		public string isDynamicEnvelope { get; set; }
		public string burnDefaultTabData { get; set; }
	}

	public class Folder
	{
		public string name { get; set; }
		public string type { get; set; }
		public Owner owner { get; set; }
		public string folderId { get; set; }
		public string uri { get; set; }
	}

	public class Owner
	{
		public string userId { get; set; }
		public string email { get; set; }
	}

	public class Page
	{
		public string pageId { get; set; }
		public string sequence { get; set; }
		public string height { get; set; }
		public string width { get; set; }
		public string dpi { get; set; }
	}

	public class DocuSignResponse
	{
		public string @event { get; set; }
		public string apiVersion { get; set; }
		public string uri { get; set; }
		public int retryCount { get; set; }
		public int configurationId { get; set; }
		public DateTime generatedDateTime { get; set; }
		public Data data { get; set; }
	}

	public class Sender
	{
		public string userName { get; set; }
		public string userId { get; set; }
		public string accountId { get; set; }
		public string email { get; set; }
	}
}