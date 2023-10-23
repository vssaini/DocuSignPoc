using System;
using System.Security.Cryptography;
using System.Text;

namespace DocuSignPoc.Services.DocuSign
{
	public static class HMACValidation
	{
		/// <summary>
		/// Validates the payload that was received in the body (raw content) of the Connect webhook message.
		/// </summary>
		/// <param name="secret">HMAC key from the Connect page in DocuSign.</param>
		/// <param name="payload">Raw Content of the webhook message without wrapping or formatting</param>
		/// <param name="signature">Value of the X-DocuSign-signature-1 header of the webhook request.</param>
		/// <returns>True if the payload was verified with the secret.</returns>
		public static bool HashIsValid(string secret, string payload, string signature)
		{
			ReadOnlySpan<byte> hashBytes = Convert.FromBase64String(ComputeHash(secret, payload));
			ReadOnlySpan<byte> verifyBytes = Convert.FromBase64String(signature);

			return CryptographicOperations.FixedTimeEquals(hashBytes, verifyBytes);
		}

		private static string ComputeHash(string secret, string payload)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(secret);
			HMAC hmac = new HMACSHA256(bytes);
			bytes = Encoding.UTF8.GetBytes(payload);

			return Convert.ToBase64String(hmac.ComputeHash(bytes));
		}
	}
}