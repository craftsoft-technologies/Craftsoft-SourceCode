using System;
using System.Security.Cryptography;
using System.Text;

namespace APP.Web.Common.Utilities
{
    public static class Encoder
    {
        private static MD5CryptoServiceProvider md5CryptoServiceProvider = new MD5CryptoServiceProvider();

        public static string HtmlEncode(string html)
        {
            var builder = new StringBuilder();
            foreach (var c in html)
            {
                switch (c)
                {
                    case '&':
                        builder.Append("&amp;");
                        break;

                    case '<':
                        builder.Append("&lt;");
                        break;

                    case '>':
                        builder.Append("&gt;");
                        break;

                    default:
                        builder.Append(c);
                        break;
                }
            }
            return builder.ToString();
        }

        public static string MD5(string input)
        {
            //Logic provide by QD
            byte[] bytes = Encoding.Default.GetBytes(input);
            byte[] hash = md5CryptoServiceProvider.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
        }

        public static string Base64Encode(string input)
        {
            //Logic provide by QD
            byte[] bytes = Encoding.Default.GetBytes(input == null ? string.Empty : input);
            return Convert.ToBase64String(bytes, 0, bytes.Length);
        }

        public static string Base64Decode(string input)
        {
            //Logic provide by QD
            byte[] bytes = Convert.FromBase64String(input == null ? string.Empty : input);
            return Encoding.Default.GetString(bytes);
        }
    }
}
