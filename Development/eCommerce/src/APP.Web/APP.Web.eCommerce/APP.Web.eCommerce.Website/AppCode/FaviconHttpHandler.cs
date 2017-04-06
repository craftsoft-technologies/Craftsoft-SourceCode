using System.Diagnostics;
using System.IO;
using System.Web;
using APP.Web.Common.Utilities;

namespace APP.Web.eCommerce.Website
{
    public class FaviconHttpHandler : IHttpHandler
    {
        static bool hasInitialized = false;
        static byte[] icoBytes;

        public static void Initialize(string partnerFavIcoName, string baseFavIcoName)
        {
            if (File.Exists(partnerFavIcoName))
            {
                LogHelper.Server.Info(" Load " + partnerFavIcoName);
                icoBytes = File.ReadAllBytes(partnerFavIcoName);
            }
            else
            {
                LogHelper.Server.Info(" Load " + baseFavIcoName);
                icoBytes = File.ReadAllBytes(baseFavIcoName);
            }
            hasInitialized = true;
        }

        #region IHttpHandler Members
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            Debug.Assert(hasInitialized, "Please call Initialize before use FaviconHttpHandler");
            context.Response.ContentType = "image/png";
            context.Response.BinaryWrite(icoBytes);
        }
        #endregion
    }
}