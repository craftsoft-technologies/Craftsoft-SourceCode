using System;
using APP.Infrastructure.SsoManagement.Modules;
using APP.Infrastructure.SsoManagement.Policies;

namespace APP.Infrastructure.SsoManagement.Common
{
    public class SSOFactory
    {
        public static ISessionModule CreateSessionModule(int websiteID)
        {
            var ret = new SessionModule();
            ret.WebsiteID = websiteID;
            return ret;
        }

        public static ISessionModule CreateSessionModule()
        {
            return CreateSessionModule(Convert.ToInt32(Configuration.AppConfiguration.Instance.ssoSetting.WebSiteID));
        }

        public static ISignInPolicy CreateSignInPolicy(int websiteID, SignInPolicyType policyType)
        {
            switch (policyType)
            {
                case SignInPolicyType.NormalSignIn:
                    return new NormalSignInPolicy(websiteID);
                case SignInPolicyType.SingleSignIn:
                    return new SingleSignInPolicy(websiteID);
                default:
                    return null;
            }
        }

        public static ISignOutPolicy CreateSignOutPolicy(int websiteID, SignOutPolicyType policyType)
        {
            switch (policyType)
            {
                case SignOutPolicyType.NormalSignOut:
                    return new NormalSignOutPolicy(websiteID);
                case SignOutPolicyType.SingleSignOut:
                    return new SingleSignOutPolicy(websiteID);
                default:
                    return null;
            }
        }
    }
}
