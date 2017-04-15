using System;
using System.Net;
using APP.Common.Utilities;
using APP.Infrastructure.SsoManagement.Common;

namespace APP.Infrastructure.SsoManagement.Cryto
{
    public class SessionExCrypto
    {
        private const string InitValue = "@1B2c3D4e5F6g7H8"; //a random string
        private const string HashAlgorithm = "SHA1";
        private const string PassPhrase = "SessionExchangeEncryption";//a random string
        private const int PasswordIteration = 2;
        private const int KeySize = 256;
        private const char Separator = '|';
        private const string TokenDateTimeFormat = "yyyyMMddHHmmss";
        private const string ValidString = "";

        public static string EncryptToken(string tokenid, DateTime currentDateTime, string clientIP)
        {
            string saltValue = "testing";//DateTime.UtcNow.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture);
            string token = SerializeToken(tokenid, currentDateTime, clientIP);
            return CryptoHelper.SymmetricKeyEncrypt(token, PassPhrase, saltValue, HashAlgorithm,
                PasswordIteration, InitValue, KeySize, true);
        }

        public static string DecryptToken(string token, out DateTime tokenDateTime, out string clientIP)
        {
            try
            {
                string saltValue = "testing";//DateTime.UtcNow.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture);
                string decryptedToken = CryptoHelper.SymmetricKeyDecrypt(token, PassPhrase, saltValue, HashAlgorithm,
                    PasswordIteration, InitValue, KeySize, true);
                return DeserializeToken(decryptedToken, out tokenDateTime, out clientIP);
            }
            catch
            {
                tokenDateTime = DateTime.MinValue;
                clientIP = string.Empty;
                return string.Empty;
            }
        }


        private static string SerializeToken(string tokenid, DateTime currentDateTime, string clientIP)
        {
            Guid guid = new Guid(tokenid);
            byte[] tokenbytes = guid.ToByteArray();
            byte[] dtbytes = BitConverter.GetBytes(currentDateTime.ToBinary());
            byte[] ipbytes = CommonHelper.ConvertIPToBytes(clientIP);
            byte[] msg = new byte[tokenbytes.Length + dtbytes.Length + ipbytes.Length];
            tokenbytes.CopyTo(msg, 0);
            dtbytes.CopyTo(msg, tokenbytes.Length);
            ipbytes.CopyTo(msg, tokenbytes.Length + dtbytes.Length);

            return Convert.ToBase64String(msg);

        }

        private static string DeserializeToken(string token, out DateTime tokenDateTime, out string clientIP)
        {
            byte[] tokenbytes = Convert.FromBase64String(token);
            byte[] guidBytes = new byte[16];
            Array.Copy(tokenbytes, guidBytes, 16);

            Guid guid = new Guid(guidBytes);
            string tokenid = guid.ToString("N");
            tokenDateTime = DateTime.FromBinary(BitConverter.ToInt64(tokenbytes, 16));
            byte[] ipbytes = new byte[tokenbytes.Length - 24];
            Array.Copy(tokenbytes, 24, ipbytes, 0, tokenbytes.Length - 24);
            clientIP = (new IPAddress(ipbytes)).ToString();
            return tokenid;

        }
    }
}
