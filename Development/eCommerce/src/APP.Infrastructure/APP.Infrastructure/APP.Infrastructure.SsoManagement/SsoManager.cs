using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using APP.Infrastructure.SsoManagement.Common;
using APP.Infrastructure.SsoManagement.Cryto;
using APP.Infrastructure.SsoManagement.Model;
using APP.Infrastructure.SsoManagement.Modules;
using APP.Infrastructure.SsoManagement.Policies;
using log4net;

namespace APP.Infrastructure.SsoManagement
{
    public class SsoManager
    {
        private ISignInPolicy signInPolicy;
        private ISignOutPolicy signOutPolicy;
        private ISessionModule sessionModule;
        private int WebsiteID;
        private static ILog iLogger = LogManager.GetLogger(typeof(SsoManager));

        private SsoManager(ISignInPolicy pSignInPolicy, ISignOutPolicy pSignOutPolicy, int pWebsiteID)
        {
            signInPolicy = pSignInPolicy;
            signOutPolicy = pSignOutPolicy;
            sessionModule = SSOFactory.CreateSessionModule(pWebsiteID);
            WebsiteID = pWebsiteID;
        }

        #region singleton

        private static SsoManager _instance;
        private static readonly object _locker = new object();
        public static SsoManager GetInstance()
        {
            if (_instance == null)
            {
                lock (_locker)
                {
                    if (_instance == null)
                    {
                        string SOPolicyConfig = Configuration.AppConfiguration.Instance.ssoSetting.SignOutPolicy;
                        string SIPolicyConfig = Configuration.AppConfiguration.Instance.ssoSetting.SignInPolicy;
                        int WebsiteIDConfig = Convert.ToInt32(Configuration.AppConfiguration.Instance.ssoSetting.WebSiteID);

                        ISignInPolicy signInPolicy = SSOFactory.CreateSignInPolicy(WebsiteIDConfig, (SignInPolicyType)Enum.Parse(typeof(SignInPolicyType), SIPolicyConfig));
                        ISignOutPolicy signOutPolicy = SSOFactory.CreateSignOutPolicy(WebsiteIDConfig, (SignOutPolicyType)Enum.Parse(typeof(SignOutPolicyType), SOPolicyConfig));
                        _instance = new SsoManager(signInPolicy, signOutPolicy, WebsiteIDConfig);
                    }
                }

            }
            return _instance;
        }

        #endregion

        #region Session Management

        public SessionInfo Login(SessionInfo session)
        {
            if (!CommonHelper.CheckLoginSession(session))
                throw new ArgumentException("some values in session has not been filled");
            //initialize the session
            return signInPolicy.Login(session);
        }

        public void Logout(string tokenid)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session.Clear();
            }

            signOutPolicy.Logout(tokenid);
        }

        public void SetSsoSessionAndStoreInCacheForWebsite()
        {
            Dictionary<string, string> dicSsoToken = new Dictionary<string, string>();
            Dictionary<string, CurrentUserInfo> dicUserInfo = new Dictionary<string, CurrentUserInfo>();


            TotalOnlineSessionInfo onlinesession = sessionModule.GetTotalOnlineUser(WebsiteID);
            if (onlinesession.TotalOnlineUser > 0)
            {
                foreach (var session in onlinesession.OnlineUser)
                {
                    string ssoToken = string.Empty;
                    ssoToken = SessionExCrypto.EncryptToken(session.TokenID, session.LastSuccessfulLogin, session.IPAddress);
                    dicSsoToken[session.TokenID] = ssoToken;

                    dicUserInfo[ssoToken] = new CurrentUserInfo { UserId = session.UserID, UserName = session.LoginName, LastSucessfulLoginTime = session.LastSuccessfulLogin, LastActivityTime = GetLastActivityTimeFromCache(ssoToken) };
                }
            }

            //add to cache
            HttpRuntime.Cache.Remove(Common.Constants.CACHEKEY_SERVERCACHE_SSOTOKEN);
            HttpRuntime.Cache.Add(Common.Constants.CACHEKEY_SERVERCACHE_SSOTOKEN, dicSsoToken, null,
                               Cache.NoAbsoluteExpiration,
                               TimeSpan.FromMinutes(Convert.ToDouble(Configuration.AppConfiguration.Instance.ssoSetting.ServerCacheExpiry)),
                               CacheItemPriority.NotRemovable, null);

            HttpRuntime.Cache.Remove(Common.Constants.CACHEKEY_SERVERCACHE_USERINFO);
            HttpRuntime.Cache.Add(Common.Constants.CACHEKEY_SERVERCACHE_USERINFO, dicUserInfo, null,
                               Cache.NoAbsoluteExpiration,
                               TimeSpan.FromMinutes(Convert.ToDouble(Configuration.AppConfiguration.Instance.ssoSetting.ServerCacheExpiry)),
                               CacheItemPriority.NotRemovable, null);


        }

        public string SetSsoToken(string tokenId, DateTime lastSucessfulLoginTime)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string ssoToken = string.Empty;
            ssoToken = SessionExCrypto.EncryptToken(tokenId, lastSucessfulLoginTime, CommonHelper.GetClientIP(HttpContext.Current.Request));

            if (HttpRuntime.Cache[Common.Constants.CACHEKEY_SERVERCACHE_SSOTOKEN] != null)
            {
                dic = HttpRuntime.Cache[Common.Constants.CACHEKEY_SERVERCACHE_SSOTOKEN] as Dictionary<string, string>;
                dic[tokenId] = ssoToken;
            }
            else
            {
                dic.Add(tokenId, ssoToken);
                HttpRuntime.Cache.Remove(Common.Constants.CACHEKEY_SERVERCACHE_SSOTOKEN);
                HttpRuntime.Cache.Add(Common.Constants.CACHEKEY_SERVERCACHE_SSOTOKEN, dic, null,
                                   Cache.NoAbsoluteExpiration,
                                   TimeSpan.FromMinutes(HttpContext.Current.Session.Timeout),
                                   CacheItemPriority.NotRemovable, null);
            }
            return ssoToken;

        }

        public void SetUserInfo(string ssoToken, int userId, string userName)
        {
            DateTime tokendate = DateTime.Now;
            string clientIP = string.Empty;
            string tokenId = SessionExCrypto.DecryptToken(ssoToken, out tokendate, out clientIP);

            Dictionary<string, CurrentUserInfo> dic = new Dictionary<string, CurrentUserInfo>();
            if (HttpRuntime.Cache[Common.Constants.CACHEKEY_SERVERCACHE_USERINFO] != null)
            {
                dic = HttpRuntime.Cache[Common.Constants.CACHEKEY_SERVERCACHE_USERINFO] as Dictionary<string, CurrentUserInfo>;
                dic[ssoToken] = new CurrentUserInfo { UserId = userId, UserName = userName, LastSucessfulLoginTime = tokendate, LastActivityTime = DateTime.Now };
            }
            else
            {
                dic.Add(ssoToken, new CurrentUserInfo { UserId = userId, UserName = userName, LastSucessfulLoginTime = tokendate, LastActivityTime = DateTime.Now });
                HttpRuntime.Cache.Remove(Common.Constants.CACHEKEY_SERVERCACHE_USERINFO);
                HttpRuntime.Cache.Add(Common.Constants.CACHEKEY_SERVERCACHE_USERINFO, dic, null,
                                   Cache.NoAbsoluteExpiration,
                                   TimeSpan.FromMinutes(HttpContext.Current.Session.Timeout),
                                   CacheItemPriority.NotRemovable, null);
            }
        }

        public void UpdateCurrentUserLastActivityTimeInCache(string ssoToken)
        {
            DateTime tokendate = DateTime.Now;
            string clientIP = string.Empty;
            string tokenId = SessionExCrypto.DecryptToken(ssoToken, out tokendate, out clientIP);

            Dictionary<string, CurrentUserInfo> dic = new Dictionary<string, CurrentUserInfo>();
            if (HttpRuntime.Cache[Common.Constants.CACHEKEY_SERVERCACHE_USERINFO] != null)
            {
                dic = HttpRuntime.Cache[Common.Constants.CACHEKEY_SERVERCACHE_USERINFO] as Dictionary<string, CurrentUserInfo>;
                dic[ssoToken] = new CurrentUserInfo { UserId = GetCurrentUserIdFromCache(ssoToken), UserName = GetCurrentUserNameFromCache(ssoToken), LastSucessfulLoginTime = tokendate, LastActivityTime = DateTime.Now };
            }
            else
            {
                dic.Add(ssoToken, new CurrentUserInfo { UserId = GetCurrentUserIdFromCache(ssoToken), UserName = GetCurrentUserNameFromCache(ssoToken), LastSucessfulLoginTime = tokendate, LastActivityTime = DateTime.Now });
                HttpRuntime.Cache.Remove(Common.Constants.CACHEKEY_SERVERCACHE_USERINFO);
                HttpRuntime.Cache.Add(Common.Constants.CACHEKEY_SERVERCACHE_USERINFO, dic, null,
                                   Cache.NoAbsoluteExpiration,
                                   TimeSpan.FromMinutes(HttpContext.Current.Session.Timeout),
                                   CacheItemPriority.NotRemovable, null);
            }
        }

        public bool IsValidSession(string ssoToken)
        {
            //validate session from token with current session 
            string clientTokenId = GetTokenIdBySsoToken(ssoToken);
            string cacheSsoToken = GetSsoTokenByTokenIDFromCache(clientTokenId);
            if (cacheSsoToken == ssoToken)
            {
                //update token last activity time
                UpdateCurrentUserLastActivityTimeInCache(ssoToken);
                return true;
            }
            return false;
        }

        public string CheckInActiveTime(string ssoToken)
        {
            System.TimeSpan diffResult = DateTime.Now.Subtract(GetLastActivityTimeFromCache(ssoToken).AddMinutes(10));
            if (diffResult.TotalMinutes > 0)
            {
                return "Session Going to expired in  5 minutes";
            }
            return "";
        }

        public DateTime GetLastSucessfulLoginTimeBySsoToken(string ssoToken)
        {
            DateTime tokendate = DateTime.Now;
            string clientIP = string.Empty;
            SessionExCrypto.DecryptToken(ssoToken, out tokendate, out clientIP);
            return tokendate;
        }

        public DateTime GetLastActivityTimeFromCache(string ssoToken)
        {
            DateTime lastActivityTime = DateTime.Now;
            CurrentUserInfo userInfo = new CurrentUserInfo();
            if (HttpRuntime.Cache[Common.Constants.CACHEKEY_SERVERCACHE_USERINFO] != null)
            {
                var dic = HttpRuntime.Cache[Common.Constants.CACHEKEY_SERVERCACHE_USERINFO] as Dictionary<string, CurrentUserInfo>;
                if (dic.ContainsKey(ssoToken))
                {
                    userInfo = (CurrentUserInfo)dic[ssoToken];
                }
            }

            if (userInfo != null)
            {
                lastActivityTime = userInfo.LastActivityTime;
            }
            return lastActivityTime;
        }

        public string GetCurrentUserNameFromCache(string ssoToken)
        {
            string name = string.Empty;
            CurrentUserInfo userInfo = new CurrentUserInfo();
            if (HttpRuntime.Cache[Common.Constants.CACHEKEY_SERVERCACHE_USERINFO] != null)
            {
                var dic = HttpRuntime.Cache[Common.Constants.CACHEKEY_SERVERCACHE_USERINFO] as Dictionary<string, CurrentUserInfo>;
                if (dic.ContainsKey(ssoToken))
                {
                    userInfo = (CurrentUserInfo)dic[ssoToken];
                }
            }

            if (userInfo != null)
            {
                name = userInfo.UserName;
            }
            return name;
        }

        public string GetSsoTokenByTokenIDFromCache(string tokenID)
        {
            string ssoToken = string.Empty;
            if (HttpRuntime.Cache[Common.Constants.CACHEKEY_SERVERCACHE_SSOTOKEN] != null)
            {
                var dic = HttpRuntime.Cache[Common.Constants.CACHEKEY_SERVERCACHE_SSOTOKEN] as Dictionary<string, string>;
                if (dic.ContainsKey(tokenID))
                {
                    ssoToken = dic[tokenID].ToString();
                }
            }
            return ssoToken;
        }

        public int GetCurrentUserIdFromCache(string ssoToken)
        {
            CurrentUserInfo userInfo = new CurrentUserInfo();
            if (HttpRuntime.Cache[Common.Constants.CACHEKEY_SERVERCACHE_USERINFO] != null)
            {
                var dic = HttpRuntime.Cache[Common.Constants.CACHEKEY_SERVERCACHE_USERINFO] as Dictionary<string, CurrentUserInfo>;
                if (dic.ContainsKey(ssoToken))
                {
                    userInfo = (CurrentUserInfo)dic[ssoToken];
                }
            }
            if (userInfo == null)
            {
                return 0;
            }
            return userInfo.UserId;
        }

        public string GetTokenIdBySsoToken(string ssoToken)
        {
            DateTime tokendate = DateTime.Now;
            string clientIP = string.Empty;
            return SessionExCrypto.DecryptToken(ssoToken, out tokendate, out clientIP);
        }

        public string GetClientIPBySsoToken(string ssoToken)
        {
            DateTime tokendate = DateTime.Now;
            string clientIP = string.Empty;
            SessionExCrypto.DecryptToken(ssoToken, out tokendate, out clientIP);
            return clientIP;
        }



        #endregion

        #region DataBase Server

        public bool CheckSessionFromDB(string tokenid)
        {
            if (string.IsNullOrEmpty(tokenid))
                throw new ArgumentNullException(tokenid);
            return sessionModule.CheckSession(tokenid);
        }

        public SessionStatus ValidateSessionFromDB(string tokenid)
        {
            if (string.IsNullOrEmpty(tokenid))
                throw new ArgumentNullException(tokenid);

            SessionInfo dto = sessionModule.GetSession(tokenid);
            if (dto != null)
            {
                if (dto.SsoOnlineUserID <= 0)
                    return SessionStatus.NotExists;
                if (dto.Remarks.ToLower() == SessionStatus.Kickout.ToString().ToLower())
                    return SessionStatus.Kickout;
                if (dto.IsLoginExpired)
                    return SessionStatus.Expired;

            }
            else
            {
                return SessionStatus.NotExists;
            }
            return SessionStatus.Valid;
        }

        public SessionInfo CheckAndGetSessionFromDB(string tokenid)
        {
            if (string.IsNullOrEmpty(tokenid))
                throw new ArgumentNullException(tokenid);
            return sessionModule.GetSession(tokenid);
        }

        public TotalOnlineSessionInfo GetTotalOnlineUser(int websiteId)
        {
            return sessionModule.GetTotalOnlineUser(websiteId);
        }

        /// <summary>
        /// Register the successful login time, this method should be called after the successfull login
        /// </summary>
        /// <param name="loginID">loginid</param>
        public void RegisterSuccessfulLoginTime(string loginID)
        {
            sessionModule.RegisterSuccessfulLoginTime(loginID);

        }

        /// <summary>
        /// Register the failed login time, this method should be called after the failed login
        /// </summary>
        /// <param name="loginID">loginid</param>
        public void RegisterFailedLoginTime(string loginID)
        {
            sessionModule.RegisterFailedLoginTime(loginID);
        }

        /// <summary>
        /// Get login time (Successful last login and failed login)
        /// </summary>
        /// <param name="loginID">login id</param>
        /// <returns>DateTime array, return[0] = Successful Last Login , return[1] = failed login
        /// if time is null, it will return DateTime.MinValue</returns>
        public DateTime[] GetLoginTime(string loginID)
        {
            return sessionModule.GetLoginTime(loginID);
        }

        public void RemoveSessionByWebsiteIdAndUserId(int websiteId, int userId)
        {
            sessionModule.RemoveSessionByWebsiteIdAndUserId(websiteId, userId);
        }

        public int UpdateMemberActivity(string loginName, int memberSiteActivityID)
        {
            return sessionModule.UpdateMemberActivity(loginName, memberSiteActivityID);

        }
        #endregion

    }
}
