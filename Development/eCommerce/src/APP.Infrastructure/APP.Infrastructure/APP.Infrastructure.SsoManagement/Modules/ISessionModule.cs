using System;
using APP.Infrastructure.SsoManagement.Model;

namespace APP.Infrastructure.SsoManagement.Modules
{
    public interface ISessionModule
    {
        int WebsiteID { get; set; }

        #region Session

        SessionInfo GetSession(string tokenID);

        string CreateSession(SessionInfo session);

        bool CheckSession(string tokenID);

        SessionInfo GetSessionByWebsiteIdAndUserId(int websiteId, int userId);

        #endregion

        #region Remove sessions

        /// <summary>
        /// Remove the session by the tokenid
        /// </summary>
        /// <param name="tokenID">the tokenid of the session</param>
        void RemoveSessionByTokenID(string tokenID);

        void RemoveSessionByWebsiteIdAndUserId(int websiteId, int userId);

        void InActivateSessionByTokenID(string tokenid);

        void InActivateSessionByWebsiteIdAndUserId(int websiteId, int userId);

        void InActivateSessionByWebsiteIdAndLoginName(int websiteId, string loginName);
        #endregion

        #region Get Online counts

        /// <summary>
        /// Get Total Online Count by website Id
        /// </summary>
        /// <param name="tokenID">the website Id of the session</param>
        TotalOnlineSessionInfo GetTotalOnlineUser(int websiteId);

        #endregion

        #region Login Time

        /// <summary>
        /// Register the successful login time, this method should be called after the successfull login
        /// </summary>
        /// <param name="loginID">loginid</param>
        void RegisterSuccessfulLoginTime(string loginID);

        /// <summary>
        /// Register the failed login time, this method should be called after the failed login
        /// </summary>
        /// <param name="loginID">loginid</param>
        void RegisterFailedLoginTime(string loginID);

        /// <summary>
        /// Get login time (Successful last login and failed login)
        /// </summary>
        /// <param name="loginID">login id</param>
        /// <returns>DateTime array, return[0] = Successful Last Login , return[1] = failed login
        /// if time is null, it will return DateTime.MinValue</returns>
        DateTime[] GetLoginTime(string loginID);

        int UpdateMemberActivity(string loginName, int memberSiteActivityID);
        #endregion

        #region Kickout Member
        void KickoutMemberOnline(string tokenid);
        #endregion
    }
}
