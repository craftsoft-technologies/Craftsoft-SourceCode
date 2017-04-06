using System;
using System.Net;
using System.Net.Sockets;
using System.Web;
using APP.Infrastructure.SsoManagement.Model;

namespace APP.Infrastructure.SsoManagement.Common
{
    public class CommonHelper
    {
        public static string GetTokenID()
        {

            object objtokenid = null;
            if (HttpContext.Current != null)
            {
                objtokenid = SsoHelper.Instance.GetTokenId();
            }
            if (objtokenid == null || objtokenid.GetType() != typeof(string))
            {
                return string.Empty;
            }
            return objtokenid.ToString();
        }

        public static void SetTokenID(string userName, string tokenId)
        {
            if (HttpContext.Current != null)
            {
                SsoHelper.Instance.SetTokenId(userName, tokenId);
            }
            else
            {
                throw new Exception("Current request is NULL");
            }
        }

        public static string GetString(object o)
        {
            if (o != null)
                return o.ToString();
            else
                return "NULL!";
        }

        public static byte[] ConvertIPToBytes(string ip)
        {
            IPAddress ipaddress;
            bool isValid = IPAddress.TryParse(ip, out ipaddress);

            if (isValid)
            {
                return ipaddress.GetAddressBytes();
            }
            return null;
        }

        public static string GetClientIP(HttpRequest Request)
        {
            if (!string.IsNullOrEmpty(Request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
            {
                foreach (string ip in Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(new char[] { ',' }))
                {
                    if (CheckIP(ip.Trim()))
                    {
                        return ip;
                    }
                }
            }
            if (CheckIP(Request.ServerVariables["HTTP_CLIENT_IP"]))
            {
                return Request.ServerVariables["HTTP_CLIENT_IP"];
            }
            if (CheckIP(Request.ServerVariables["HTTP_X_FORWARDED"]))
            {
                return Request.ServerVariables["HTTP_X_FORWARDED"];
            }
            else if (CheckIP(Request.ServerVariables["HTTP_X_CLUSTER_CLIENT_IP"]))
            {
                return Request.ServerVariables["HTTP_X_CLUSTER_CLIENT_IP"];
            }
            else if (CheckIP(Request.ServerVariables["HTTP_FORWARDED_FOR"]))
            {
                return Request.ServerVariables["HTTP_FORWARDED_FOR"];
            }
            else if (CheckIP(Request.ServerVariables["HTTP_FORWARDED"]))
            {
                return Request.ServerVariables["HTTP_FORWARDED"];
            }
            else
            {
                return Request.ServerVariables["REMOTE_ADDR"];
            }
        }

        public static string GetClientIP()
        {

            HttpRequest Request = HttpContext.Current.Request;
            if (!string.IsNullOrEmpty(Request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
            {
                foreach (string ip in Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(new char[] { ',' }))
                {
                    if (CheckIP(ip.Trim()))
                    {
                        return ip;
                    }
                }
            }
            if (CheckIP(Request.ServerVariables["HTTP_CLIENT_IP"]))
            {
                return Request.ServerVariables["HTTP_CLIENT_IP"];
            }
            if (CheckIP(Request.ServerVariables["HTTP_X_FORWARDED"]))
            {
                return Request.ServerVariables["HTTP_X_FORWARDED"];
            }
            else if (CheckIP(Request.ServerVariables["HTTP_X_CLUSTER_CLIENT_IP"]))
            {
                return Request.ServerVariables["HTTP_X_CLUSTER_CLIENT_IP"];
            }
            else if (CheckIP(Request.ServerVariables["HTTP_FORWARDED_FOR"]))
            {
                return Request.ServerVariables["HTTP_FORWARDED_FOR"];
            }
            else if (CheckIP(Request.ServerVariables["HTTP_FORWARDED"]))
            {
                return Request.ServerVariables["HTTP_FORWARDED"];
            }
            else
            {
                return Request.ServerVariables["REMOTE_ADDR"];
            }
        }

        public static bool CheckIP(string addr)
        {
            if (string.IsNullOrEmpty(addr))
                return false;
            IPAddress ipadd;
            return IPAddress.TryParse(addr, out ipadd);
        }

        public static string GetProperIP(string addr)
        {
            IPAddress ipadd;
            if (IPAddress.TryParse(addr, out ipadd))
            {
                if (ipadd.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    //this is to remove the scope id for IPv6
                    //IPv6 scope id is not captured when to byte[]
                    //thus we cannot put the scopeid into passport string
                    ipadd.ScopeId = 0;
                }
                return ipadd.ToString();
            }
            else
            {
                return null;
            }
        }

        public static bool CheckLoginSession(SessionInfo session)
        {
            if (session == null)
                return false;

            if (string.IsNullOrEmpty(session.LoginName))
                return false;

            return true;
        }
    }
}
