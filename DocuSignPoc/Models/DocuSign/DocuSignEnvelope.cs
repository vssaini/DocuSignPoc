namespace DocuSignPoc.Models.DocuSign
{
	public class DocuSignEnvelope
	{
		public string SignerEmail { get; set; }
		public string SignerName { get; set; }
		public string FileNameWithExtension { get; set; }
		public byte[] FileBytes { get; set; }
		public int FileId { get; set; }
	}
}