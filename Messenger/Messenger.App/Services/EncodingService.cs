using System.Security.Cryptography;
using System.Text;

namespace Messenger.App.Services
{
    public class EncodingService
    {
        private static byte[] _keyArray = ASCIIEncoding.ASCII.GetBytes("IRUENVHSORUQKAHFNRUWJDVX");
        private static byte[] _ivArray = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };

        public static string GetHashString(string s)
        {
            var bytes = Encoding.Unicode.GetBytes(s);
            var hash = string.Empty;

            using (var CSP = MD5.Create())
            {
                var byteHash = CSP.ComputeHash(bytes);
                foreach (byte b in byteHash)
                {
                    hash += string.Format("{0:x2}", b);
                }
            }
            
            return hash;
        }

        public static string Encrypt(string data)
        {
            var toEncryptArray = UTF8Encoding.UTF8.GetBytes(data);

            using (var tripleDESalg = TripleDES.Create())
            {
                tripleDESalg.Key = _keyArray;
                tripleDESalg.IV = _ivArray;
                tripleDESalg.Mode = CipherMode.ECB;
                var encryptor = tripleDESalg.CreateEncryptor(_keyArray, _ivArray);
                var resultArray = encryptor.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                tripleDESalg.Clear();
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
        }

        public static string Decrypt(string data)
        {
            var toDecryptArray = Convert.FromBase64String(data);

            using (var tripleDESalg = TripleDES.Create())
            {
                tripleDESalg.Key = _keyArray;
                tripleDESalg.IV = _ivArray;
                tripleDESalg.Mode = CipherMode.ECB;
                var encryptor = tripleDESalg.CreateDecryptor(_keyArray, _ivArray);
                var resultArray = encryptor.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);            
                tripleDESalg.Clear();
                return UTF8Encoding.UTF8.GetString(resultArray);
            }
        }
    }
}