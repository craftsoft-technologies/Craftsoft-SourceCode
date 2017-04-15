using System;
using System.Diagnostics;
using System.Resources;
using APP.Entities.Enum;
using APP.Web.Common.Properties;

namespace APP.Web.Common.Utilities
{
    public static class ErrorMessageUtility
    {
        private static readonly ResourceManager ResourceManager = new ResourceManager(typeof(ErrorMessage));

        public static string GetLocalizedErrorMessage(string errorCode)
        {
            return ResourceManager.GetString(errorCode);

            //return resourceManager.GetString(Enum.GetName(typeof(ErrorEnum.ErrCode), errorCode));
        }

        [Conditional("DEBUG")]
        public static void ValidResources()
        {
            var errorMessageResourceManager = new ResourceManager(typeof(ErrorMessage));
            var errorCodes = Enum.GetNames(typeof(ErrorEnum.ErrCode));
            foreach (var errorCode in errorCodes)
            {
                var resource = errorMessageResourceManager.GetString(errorCode);
                if (resource == null)
                {
                    throw new Exception("Please add [ErrorEnum.ErrCode." + errorCode + "] to APP.Web.Common.Properties.ErrorMessage.resx");
                }
            }
        }
    }
}
