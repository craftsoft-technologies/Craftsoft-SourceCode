using System;
using System.Net.Http;
using System.Web.Http.Filters;
using Newtonsoft.Json;
using APP.Web.eCommerce.BLL.Configuration;

namespace APP.Web.eCommerce.Website.AppCode.API
{
    public class EncryptResponseActionFilter : ActionFilterAttribute
    {
        public string Key { get; set; }
        public string IV { get; set; }

        public EncryptResponseActionFilter()
        {
            Key = "";//GCT.Manager.Integration.Configuration.AppConfiguration.Instance.casinoConfig.R88EncryptionKey;
            IV = "";//GCT.Manager.Integration.Configuration.AppConfiguration.Instance.casinoConfig.R88EncryptionIV;
        }

        /*public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (AppConfigManager.SystemSetting.IsAPIEncryptionActivated)
            {
                ResponseResult<string> newRes = new ResponseResult<string>();

                ObjectContent oldResponse = (ObjectContent)actionExecutedContext.Response.Content;
                object res = oldResponse.Value;

                string timeStamp = res.GetType().GetProperty(GCT.Common.Utilities.ReflectionHelper.GetPropertyName(() => newRes.TimeStamp)).GetValue(res, null).ToString();
                string message = res.GetType().GetProperty(GCT.Common.Utilities.ReflectionHelper.GetPropertyName(() => newRes.Message)).GetValue(res, null).ToString();
                string code = res.GetType().GetProperty(GCT.Common.Utilities.ReflectionHelper.GetPropertyName(() => newRes.Code)).GetValue(res, null).ToString();
                var body = res.GetType().GetProperty(GCT.Common.Utilities.ReflectionHelper.GetPropertyName(() => newRes.Body)).GetValue(res, null);
                string plainBody = JsonConvert.SerializeObject(body);
                string encryptedBody = AESEnDecryption.EncryptAES256(plainBody, Key, IV);

                newRes = new ResponseResult<string>(timeStamp, code, message, encryptedBody);

                System.Net.Http.ObjectContent newResponse = new ObjectContent<ResponseResult<string>>(newRes, oldResponse.Formatter);

                actionExecutedContext.Response.Content = newResponse;
            }

            base.OnActionExecuted(actionExecutedContext);
        }*/
    }
}