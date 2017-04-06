using System;
using System.Security.Cryptography;
using System.Text;

namespace APP.Web.eCommerce.Website.AppCode.API
{
    public class AESEnDecryption
    {
        /// <summary>
        /// Use AES 256 to encrypt
        /// </summary>
        /// <param name="source">This article</param>
        /// <param name="key">Because it is 256 so your password must be 32 words = 32 * 8 = 256</param>
        /// <param name="iv">IV is 128 * 16 * 8 = 128</param>
        /// <returns></returns>

        public static string EncryptAES256(string source, string key, string iv)
        {
            var MD5 = new MD5CryptoServiceProvider();
            byte[] sourceBytes = Encoding.UTF8.GetBytes(source);
            var aes = new RijndaelManaged();
            aes.Key = Encoding.UTF8.GetBytes(key);
            //aes.Key = MD5.ComputeHash(Encoding.UTF8.GetBytes(key));
            aes.IV = Encoding.UTF8.GetBytes(iv);
            //aes.IV = MD5.ComputeHash(Encoding.UTF8.GetBytes(iv));
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            ICryptoTransform transform = aes.CreateEncryptor();
            return Convert.ToBase64String(transform.TransformFinalBlock(sourceBytes, 0, sourceBytes.Length));

        }

        /// <summary>
        /// Use AES 256 to decrypt
        /// </summary>
        /// <param name="encryptData">Base64's encrypted string</param>
        /// <param name="key">Because it is 256 so your password must be 32 words = 32 * 8 = 256</param>
        /// <param name="iv">IV is 128 * 16 * 8 = 128</param>
        /// <returns></returns>
        public static string DecryptAES256(string encryptData, string key, string iv)
        {
            var encryptBytes = Convert.FromBase64String(encryptData);
            var aes = new RijndaelManaged();
            var MD5 = new MD5CryptoServiceProvider();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = Encoding.UTF8.GetBytes(iv);
            //aes.Key = MD5.ComputeHash(Encoding.UTF8.GetBytes(key));
            //aes.IV = MD5.ComputeHash(Encoding.UTF8.GetBytes(iv));

            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            ICryptoTransform transform = aes.CreateDecryptor();
            return Encoding.UTF8.GetString(transform.TransformFinalBlock(encryptBytes, 0, encryptBytes.Length));
        }

        public static string EnpryptAES(string plainText, string aeskey, string ivkey)
        {
            var AES = new RijndaelManaged();
            var MD5 = new MD5CryptoServiceProvider();
            byte[] plainTextData = Encoding.UTF8.GetBytes(plainText);
            byte[] keyData = MD5.ComputeHash(Encoding.UTF8.GetBytes(aeskey));
            byte[] IVData = MD5.ComputeHash(Encoding.UTF8.GetBytes(ivkey));
            //string xx = Convert.ToBase64String(MD5.ComputeHash(Encoding.UTF8.GetBytes(aeskey)));
            ICryptoTransform transform = AES.CreateEncryptor(keyData, IVData);
            byte[] outputData = transform.TransformFinalBlock(plainTextData, 0, plainTextData.Length);
            return Convert.ToBase64String(outputData);
        }

        public static string DecryptAES(string plainText, string aeskey, string ivkey)
        {
            var AES = new RijndaelManaged();
            var MD5 = new MD5CryptoServiceProvider();
            var cipherTextData = Convert.FromBase64String(plainText);
            byte[] keyData = MD5.ComputeHash(Encoding.UTF8.GetBytes(aeskey));
            byte[] IVData = MD5.ComputeHash(Encoding.UTF8.GetBytes(ivkey));
            ICryptoTransform transform = AES.CreateDecryptor(keyData, IVData);
            byte[] outputData = transform.TransformFinalBlock(cipherTextData, 0, cipherTextData.Length);
            return Encoding.UTF8.GetString(outputData);
        }

        public static string GetMd5Hash(string input)
        {
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

    }
}