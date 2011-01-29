using System;

namespace CryptoCA
{
	/// <summary>
	/// Summary description for CryptLib.
	/// </summary>
	public class CryptLib
	{
		private CryptLib()
		{
		}

		public static string SHA1HashBase64(string ClearText)
		{
			System.Security.Cryptography.SHA1CryptoServiceProvider Hasher = 
				new System.Security.Cryptography.SHA1CryptoServiceProvider();
	
			byte[] bClear = new byte[ClearText.Length*2];
			for (int i = 0;i<ClearText.Length*2;++i)
				bClear[i] = Convert.ToByte((((i&1)==0)?Convert.ToUInt16(ClearText[i/2])&0xFF:Convert.ToUInt16(ClearText[i/2])>>8));
			
			Hasher.ComputeHash(bClear);
			byte[] bHash = Hasher.Hash;

			return Convert.ToBase64String(bHash);
		}
	}
}
