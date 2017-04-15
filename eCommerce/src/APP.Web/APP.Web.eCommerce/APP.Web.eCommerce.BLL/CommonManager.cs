using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using APP.Web.eCommerce.BLL.Configuration;
using APP.Infrastructure.SsoManagement.Common;

namespace APP.Web.eCommerce.BLL
{
    public class CommonManager
    {
        public static string GetIP(HttpRequest request)
        {
            return CommonHelper.GetClientIP(request);
        }

        public static string GetIP()
        {
            return CommonHelper.GetClientIP();
        }

        public static int GetCNYPromotionResultByLogInName(string loginName)
        {
            int intResult = 4;
            return intResult;
        }

        public static bool CheckDateTimeFormat(string dateTime)
        {
            bool result = true;
            try
            {
                DateTime dd = Convert.ToDateTime(dateTime);
            }
            catch
            {
                result = false;
            }
            return result;
        }
    }
}
