using System;

namespace APP.Infrastructure.SsoManagement.Model
{
    [Serializable]
    public class SsoSetting
    {
        //<add key="WebSiteID" value="1" />
        public string WebSiteID { get; set; }

        //<add key="SignInPolicy" value="SingleSignIn" />
        public string SignInPolicy { get; set; }

        //<add key="SignOutPolicy" value="NormalSignOut" />
        public string SignOutPolicy { get; set; }

        //<add key="ServerCacheExpiry" value="60" />
        public string ServerCacheExpiry { get; set; }

    }
}
