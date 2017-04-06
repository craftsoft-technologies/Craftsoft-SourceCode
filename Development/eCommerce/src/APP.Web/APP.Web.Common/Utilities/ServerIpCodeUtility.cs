using System;
using System.Net;
using System.Text;
using System.Web;
using System.Linq;
using APP.Common;

namespace APP.Web.Common.Utilities
{
    /// <summary>
    /// Copy from STAR
    /// Get encoded server IP code
    /// </summary>
    public static class ServerIpCodeUtility
    {
        private static string _code;

        /// <summary>
        /// Copy from STAR
        /// Gets IP address from DNS Classes and Encodes it using EncodeNumber method. This is used to identify the correct web server in production.
        /// </summary>
        /// <returns>Encoded IP Address</returns>
        public static string GetCode()
        {
            if (!string.IsNullOrEmpty(_code))
            {
                return _code;
            }
            try
            {
                string encodedIpAddr = string.Empty;
                string ipAddr = GetIPNumberCode();
                for (int i = 0; i < ipAddr.Length; i++)
                {
                    encodedIpAddr = string.Concat(encodedIpAddr, EncodeNumber(ipAddr.Substring(i, 1)));
                }
                _code = encodedIpAddr;
            }
            catch (Exception ex)
            {
                //Log.Exception(ex);
                LogHelper.Server.Warn("ServerIpCodeUtility.GetCode exception" + GetIPNumberCode(), ex);
                _code = string.Empty;
            }
            return _code;
        }

        /// <summary>
        /// Copy from STAR
        /// Fetches Local machine IP address using DNS Class
        /// </summary>
        /// <returns>IP address in string format. If multiple IP found, it will be separated by New Line character.</returns>
        private static string GetIPNumberCode()
        {
            var builder = new StringBuilder();

            int lengthStartIndex = 6;
            string machineName = Dns.GetHostName();
            //Log.Debug("(GetLocalIPAddress) Machine Name is {0}", machineName);
            IPAddress[] iPAddresses = Dns.GetHostAddresses(machineName);

            if (iPAddresses.Length == 0)
                return string.Empty;

            //if (iPAddresses.Length == 1)
            //    return FormatIpAddress(iPAddresses[0].ToString()).Substring(lengthStartIndex);

            foreach (IPAddress ip in iPAddresses)
            {
                //Log.Debug("(GetLocalIPAddress) IP Address {0} :", ip);
                if (!ip.ToString().Contains("::1"))
                {
                    builder.AppendLine(FormatIpAddress(ip.ToString()).Substring(lengthStartIndex));
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Copy from STAR
        /// formats the IP address using AddLeadingZero Method.
        /// </summary>
        /// <param name="ipAddress">IP Address, which needs to be formatted.</param>
        /// <returns>Formatted ip address</returns>
        private static string FormatIpAddress(string ipAddress)
        {
            var builder = new StringBuilder();
            if (ipAddress.Length == 0)
                return string.Empty;

            foreach (string ipPart in ipAddress.Split('.'))
            {
                builder.Append(AddLeadingZero(ipPart));
            }

            return builder.ToString();
        }

        /// <summary>
        /// Copy from STAR
        /// Adds leading zero's. if IP part is like 1 then will be formatted as 001.
        /// </summary>
        /// <param name="numberStr">Part of the IP needs to be formated.</param>
        /// <returns>Formatted ip Part.</returns>
        private static string AddLeadingZero(string numberStr)
        {
            if (string.IsNullOrEmpty(numberStr) || numberStr.Length == 0)
                return string.Empty;

            if (numberStr.Length == 1)
                return string.Concat("0", "0", numberStr);

            if (numberStr.Length == 2)
                return string.Concat("0", numberStr);

            return numberStr;
        }

        /// <summary>
        /// Encodes IP address with very simple method.
        /// </summary>
        /// <returns>Encoded IP Address</returns>
        private static string EncodeNumber(string singleNumber)
        {
            switch (singleNumber)
            {
                case "0":
                    return "A";

                case "1":
                    return "B";

                case "2":
                    return "C";

                case "3":
                    return "D";

                case "4":
                    return "E";

                case "5":
                    return "F";

                case "6":
                    return "G";

                case "7":
                    return "H";

                case "8":
                    return "I";

                case "9":
                    return "J";

                default:
                    return " ";
            }
        }

        public static string GetProtocol()
        {
            var context = HttpContext.Current;
            Mandate.That(context != null,
                () => new InvalidOperationException("You have to execute this method in web application."));

            if (context.Request.Headers.AllKeys.Any(key => key.Equals("X-UrlSchema", StringComparison.Ordinal)) ||
                context.Request.Url.Scheme.Equals("https", StringComparison.Ordinal))
                return "https";

            return "http";
        }

        public static string GetDomainName()
        {
            //Get domain url and domain name. Example toutou.com and http://ld.pff.toutou.com

            if (HttpContext.Current.Request.Url.Host.ToLower() == "localhost")
                return "toutou.dev";

            var authorityURL = HttpContext.Current.Request.Url.Authority;
            var splitAuthorityURL = authorityURL.Split(':')[0].Split('.');
            return (splitAuthorityURL.Length > 1) ? splitAuthorityURL[splitAuthorityURL.Length - 2] + "." + splitAuthorityURL[splitAuthorityURL.Length - 1] : authorityURL;
        }

    }
}
