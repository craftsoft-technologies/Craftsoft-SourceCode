using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Newtonsoft.Json;
using APP.Web.eCommerce.BLL.Configuration;

namespace APP.Web.eCommerce.Website.AppCode.API
{
    public class DecryptRequestActionFilter : ActionFilterAttribute
    {
        public string Key { get; set; }
        public string IV { get; set; }

        public DecryptRequestActionFilter()
        {
            Key = "";//APP.Manager.Integration.Configuration.AppConfiguration.Instance.casinoConfig.R88EncryptionKey;
            IV = "";//GCT.Manager.Integration.Configuration.AppConfiguration.Instance.casinoConfig.R88EncryptionIV;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (AppConfigManager.SystemSetting.IsAPIEncryptionActivated)
            {
                string rawRequest = GetRequestBody(actionContext.Request.Content);
                string ecryptedBody = string.Empty;
                string body = string.Empty;

#if DEBUG
                ecryptedBody = "rrr";
#endif

#if DEBUG
                //Dummy data
                StringBuilder builder = new StringBuilder();
                builder.Append('{');
                builder.Append(string.Format("\"{0}\" : \"{1}\",", "loginName", "tests3"));
                builder.Append(string.Format("\"{0}\" : \"{1}\",", "password", "test3"));
                builder.Append(string.Format("\"{0}\" : \"{1}\",", "lightSpeedToken", "test3"));
                builder.Append(string.Format("\"{0}\" : \"{1}\",", "blackbox", "test3"));
                builder.Append(string.Format("\"{0}\" : \"{1}\",", "deviceID", "test3"));
                builder.Append(string.Format("\"{0}\" : \"{1}\"", "appOsType", "1"));
                builder.Append('}');
                body = builder.ToString();
#else
            RequestParam<string> paramEncrypted = JsonConvert.DeserializeObject<RequestParam<string>>(rawRequest);
            ecryptedBody = paramEncrypted.Body;
            body = AESEnDecryption.DecryptAES256(ecryptedBody, Key, IV);
#endif

                string plaintext = rawRequest.Replace("\"" + ecryptedBody + "\"", body);

                HttpParameterDescriptor param = actionContext.ActionDescriptor.GetParameters().FirstOrDefault();

                if (param != null)
                {
                    Type typeParam = param.ParameterType;
                    object poco = JsonConvert.DeserializeObject(plaintext, typeParam);
                    actionContext.ActionArguments["param"] = poco;
                }
            }

            base.OnActionExecuting(actionContext);
        }

        private string GetRequestBody(System.Net.Http.HttpContent content)
        {
            string result = string.Empty;
            using (var stream = new StreamReader(content.ReadAsStreamAsync().Result))
            {
                stream.BaseStream.Position = 0;
                result = stream.ReadToEnd();
            }
            return result;
        }
    }
}