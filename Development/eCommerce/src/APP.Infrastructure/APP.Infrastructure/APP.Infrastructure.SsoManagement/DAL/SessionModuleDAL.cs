using System;
using System.Collections.Generic;
using System.Data;
using APP.Common.DAL;
using APP.Infrastructure.ConnectionStringManagement;
using APP.Infrastructure.SsoManagement.Common;
using APP.Infrastructure.SsoManagement.Model;

namespace APP.Infrastructure.SsoManagement.DAL
{
    public class SessionModuleDAL
    {
        public static int GetTotalOnlineCount(int websiteID)
        {
            DBSettingEntity dbSetting = new DBSettingEntity(DataProvider.SqlServer, ConnectionStringManager.GetConnectionString(ConnectionStringEnum.DBSource.APP), CommandType.StoredProcedure, "[dbo].[Security_GetWebSiteTotalOnlineCount]", false);
            dbSetting.AddParameters("@WebsiteID", websiteID, ParameterDirection.Input);

            return (int)DBManager2.Instance.ExecuteScalar(dbSetting);
        }

        public static List<SessionInfo> GetWebSiteTotalOnlineUser(int websiteID)
        {
            List<SessionInfo> listResult = new List<SessionInfo>();

            DBSettingEntity dbSetting = new DBSettingEntity(DataProvider.SqlServer, ConnectionStringManager.GetConnectionString(ConnectionStringEnum.DBSource.APP), CommandType.StoredProcedure, "[dbo].[Security_GetWebSiteTotalOnlineUser]", false);
            dbSetting.AddParameters("@WebsiteID", websiteID, ParameterDirection.Input);

            Func<IDataReader, bool> func = delegate (IDataReader dr)
            {
                while (dr.Read())
                {
                    SessionInfo subResult = new SessionInfo();
                    subResult.SsoOnlineUserID = DataTypeHelper.GetInt(dr["SsoOnlineUserID"]);
                    subResult.WebsiteID = DataTypeHelper.GetInt(dr["WebsiteID"]);
                    subResult.LoginName = DataTypeHelper.GetString(dr["LoginName"]);
                    subResult.UserID = DataTypeHelper.GetInt(dr["UserID"]);
                    subResult.TokenID = DataTypeHelper.GetString(dr["TokenID"]);
                    subResult.IPAddress = DataTypeHelper.GetString(dr["IPAddress"]);
                    subResult.Remarks = DataTypeHelper.GetString(dr["Remarks"]);
                    subResult.LastSuccessfulLogin = DataTypeHelper.GetDate(dr["LastSuccessfulLogin"]);
                    subResult.LastFailedLogin = DataTypeHelper.GetDate(dr["LastFailedLogin"]);
                    subResult.LastActivitiesTime = DataTypeHelper.GetDate(dr["LastActivitiesTime"]);
                    subResult.DateCreated = DataTypeHelper.GetDate(dr["DateCreated"]);
                    subResult.DateUpdated = DataTypeHelper.GetDate(dr["DateUpdated"]);
                    subResult.IsLoginExpired = DataTypeHelper.GetBoolean(dr["IsLoginExpired"]);
                    subResult.LoginAttempt = DataTypeHelper.GetInt(dr["LoginAttempt"]);
                    subResult.TotalOnlineUserCount = DataTypeHelper.GetInt(dr["TotalOnlineUserCount"]);
                    listResult.Add(subResult);
                }
                return true;
            };

            DBManager2.Instance.ExecuteReader(dbSetting, func);
            return listResult;

        }

        public static bool CheckSession(string tokenId)
        {
            SessionInfo dto = GetSession(tokenId);
            if (dto != null)
            {
                if (!dto.IsLoginExpired && dto.SsoOnlineUserID > 0)
                    return true;
            }
            return false;
        }

        public static SessionInfo GetSession(string tokenId)
        {
            SessionInfo subResult = new SessionInfo();

            DBSettingEntity dbSetting = new DBSettingEntity(DataProvider.SqlServer, ConnectionStringManager.GetConnectionString(ConnectionStringEnum.DBSource.APP), CommandType.StoredProcedure, "[dbo].[Security_GetSsoSession]", false);
            dbSetting.AddParameters("@TokenID", tokenId, ParameterDirection.Input);

            Func<IDataReader, bool> func = delegate (IDataReader dr)
            {
                if (dr.Read())
                {
                    subResult.SsoOnlineUserID = DataTypeHelper.GetInt(dr["SsoOnlineUserID"]);
                    subResult.WebsiteID = DataTypeHelper.GetInt(dr["WebsiteID"]);
                    subResult.LoginName = DataTypeHelper.GetString(dr["LoginName"]);
                    subResult.UserID = DataTypeHelper.GetInt(dr["UserID"]);
                    subResult.TokenID = DataTypeHelper.GetString(dr["TokenID"]);
                    subResult.IPAddress = DataTypeHelper.GetString(dr["IPAddress"]);
                    subResult.Remarks = DataTypeHelper.GetString(dr["Remarks"]);
                    subResult.LastSuccessfulLogin = DataTypeHelper.GetDate(dr["LastSuccessfulLogin"]);
                    subResult.LastFailedLogin = DataTypeHelper.GetDate(dr["LastFailedLogin"]);
                    subResult.LastActivitiesTime = DataTypeHelper.GetDate(dr["LastActivitiesTime"]);
                    subResult.DateCreated = DataTypeHelper.GetDate(dr["DateCreated"]);
                    subResult.DateUpdated = DataTypeHelper.GetDate(dr["DateUpdated"]);
                    subResult.IsLoginExpired = DataTypeHelper.GetBoolean(dr["IsLoginExpired"]);
                    subResult.LoginAttempt = DataTypeHelper.GetInt(dr["LoginAttempt"]);
                }
                return true;
            };

            DBManager2.Instance.ExecuteReader(dbSetting, func);

            return subResult;

        }

        public static SessionInfo GetSessionByWebsiteIdAndUserId(int websiteId, int userId)
        {
            SessionInfo subResult = new SessionInfo();

            DBSettingEntity dbSetting = new DBSettingEntity(DataProvider.SqlServer, ConnectionStringManager.GetConnectionString(ConnectionStringEnum.DBSource.APP), CommandType.StoredProcedure, "[dbo].[Security_GetSsoSessionByWebsiteAndUser]", false);
            dbSetting.AddParameters("@WebsiteID", websiteId, ParameterDirection.Input);
            dbSetting.AddParameters("@UserID", userId, ParameterDirection.Input);

            Func<IDataReader, bool> func = delegate (IDataReader dr)
            {
                if (dr.Read())
                {
                    subResult.SsoOnlineUserID = DataTypeHelper.GetInt(dr["SsoOnlineUserID"]);
                    subResult.LoginName = DataTypeHelper.GetString(dr["LoginName"]);
                    subResult.UserID = DataTypeHelper.GetInt(dr["UserID"]);
                    subResult.TokenID = DataTypeHelper.GetString(dr["TokenID"]);
                    subResult.IPAddress = DataTypeHelper.GetString(dr["IPAddress"]);
                    subResult.Remarks = DataTypeHelper.GetString(dr["Remarks"]);
                    subResult.LastSuccessfulLogin = DataTypeHelper.GetDate(dr["LastSuccessfulLogin"]);
                    subResult.LastFailedLogin = DataTypeHelper.GetDate(dr["LastFailedLogin"]);
                    subResult.LastActivitiesTime = DataTypeHelper.GetDate(dr["LastActivitiesTime"]);
                    subResult.DateCreated = DataTypeHelper.GetDate(dr["DateCreated"]);
                    subResult.DateUpdated = DataTypeHelper.GetDate(dr["DateUpdated"]);
                    subResult.IsLoginExpired = DataTypeHelper.GetBoolean(dr["IsLoginExpired"]);
                    subResult.LoginAttempt = DataTypeHelper.GetInt(dr["LoginAttempt"]);

                }
                return true;
            };

            DBManager2.Instance.ExecuteReader(dbSetting, func);

            return subResult;

        }

        public static void InActivateSessionByTokenID(string tokenid)
        {
            DBSettingEntity dbSetting = new DBSettingEntity(DataProvider.SqlServer, ConnectionStringManager.GetConnectionString(ConnectionStringEnum.DBSource.APP), CommandType.StoredProcedure, "[dbo].[Security_InActivateSession]", false);
            dbSetting.AddParameters("@TokenID", tokenid, ParameterDirection.Input);

            DBManager2.Instance.ExecuteNonQuery(dbSetting);

        }

        public static void InActivateSessionByWebsiteIdAndUserId(int websiteId, int userId)
        {
            DBSettingEntity dbSetting = new DBSettingEntity(DataProvider.SqlServer, ConnectionStringManager.GetConnectionString(ConnectionStringEnum.DBSource.APP), CommandType.StoredProcedure, "[dbo].[Security_InActivateSessionByWebSiteIDAndUserID]", false);
            dbSetting.AddParameters("@WebsiteID", websiteId, ParameterDirection.Input);
            dbSetting.AddParameters("@UserID", userId, ParameterDirection.Input);
            DBManager2.Instance.ExecuteNonQuery(dbSetting);
        }

        public static void InActivateSessionByWebsiteIdAndLoginName(int websiteId, string loginName)
        {
            DBSettingEntity dbSetting = new DBSettingEntity(DataProvider.SqlServer, ConnectionStringManager.GetConnectionString(ConnectionStringEnum.DBSource.APP), CommandType.StoredProcedure, "[dbo].[Security_InActivateSessionByWebSiteIDAndLoginName]", false);
            dbSetting.AddParameters("@WebsiteID", websiteId, ParameterDirection.Input);
            dbSetting.AddParameters("@LoginName", loginName, ParameterDirection.Input);
            DBManager2.Instance.ExecuteNonQuery(dbSetting);
        }

        public static void RemoveSessionByWebsiteIdAndUserId(int websiteId, int userId)
        {
            DBSettingEntity dbSetting = new DBSettingEntity(DataProvider.SqlServer, ConnectionStringManager.GetConnectionString(ConnectionStringEnum.DBSource.APP), CommandType.StoredProcedure, "[dbo].[Security_RemoveSessionByWebSiteIDAndUserID]", false);
            dbSetting.AddParameters("@WebsiteID", websiteId, ParameterDirection.Input);
            dbSetting.AddParameters("@UserID", userId, ParameterDirection.Input);
            DBManager2.Instance.ExecuteNonQuery(dbSetting);
        }

        public static void CreateSession(SessionInfo session)
        {
            DBSettingEntity dbSetting = new DBSettingEntity(DataProvider.SqlServer, ConnectionStringManager.GetConnectionString(ConnectionStringEnum.DBSource.APP), CommandType.StoredProcedure, "[dbo].[Security_CreateSsoUserSession]", false);
            dbSetting.AddParameters("@WebsiteID", session.WebsiteID, ParameterDirection.Input);
            dbSetting.AddParameters("@LoginName", session.LoginName, ParameterDirection.Input);
            dbSetting.AddParameters("@UserID", session.UserID, ParameterDirection.Input);
            dbSetting.AddParameters("@TokenID", session.TokenID, ParameterDirection.Input);
            dbSetting.AddParameters("@IPAddress", session.IPAddress, ParameterDirection.Input);
            dbSetting.AddParameters("@Channel", session.Channel, ParameterDirection.Input);
            dbSetting.AddParameters("@ServerIP", session.ServerIP, ParameterDirection.Input);
            dbSetting.AddParameters("@MemberSiteActivityID", session.MemberSiteActivityID, ParameterDirection.Input);

            DBManager2.Instance.ExecuteNonQuery(dbSetting);
        }

        public static void RemoveSessionByTokenID(string tokenid)
        {
            DBSettingEntity dbSetting = new DBSettingEntity(DataProvider.SqlServer, ConnectionStringManager.GetConnectionString(ConnectionStringEnum.DBSource.APP), CommandType.StoredProcedure, "[dbo].[Security_RemoveSessionByTokenID]", false);
            dbSetting.AddParameters("@TokenID", tokenid, ParameterDirection.Input);

            DBManager2.Instance.ExecuteNonQuery(dbSetting);
        }


        public static void RegisterSuccessfulLoginTime(string loginID, int websiteID)
        {
            RegisterLoginTime(loginID, websiteID, "[dbo].[GNR_RegisterSuccessfulLogin]");
        }

        public static void RegisterFailedLoginTime(string loginID, int websiteID)
        {
            RegisterLoginTime(loginID, websiteID, "[dbo].[GNR_RegisterFailedLogin]");
        }

        private static void RegisterLoginTime(string loginID, int websiteID, string spName)
        {
            DBSettingEntity dbSetting = new DBSettingEntity(DataProvider.SqlServer, ConnectionStringManager.GetConnectionString(ConnectionStringEnum.DBSource.APP), CommandType.StoredProcedure, spName, false);
            dbSetting.AddParameters("@LoginID", loginID, ParameterDirection.Input);
            dbSetting.AddParameters("@websiteID", websiteID, ParameterDirection.Input);

            DBManager2.Instance.ExecuteNonQuery(dbSetting);
        }

        public static DateTime[] GetLoginTime(string loginID, int websiteID)
        {
            DateTime[] results = new DateTime[3];

            DBSettingEntity dbSetting = new DBSettingEntity(DataProvider.SqlServer, ConnectionStringManager.GetConnectionString(ConnectionStringEnum.DBSource.APP), CommandType.StoredProcedure, "[dbo].[GNR_GetLoginTime]", false);
            dbSetting.AddParameters("@LoginID", loginID, ParameterDirection.Input);
            dbSetting.AddParameters("@WebsiteID", websiteID, ParameterDirection.Input);


            Func<IDataReader, bool> func = delegate (IDataReader dr)
            {
                if (dr.Read())
                {
                    results[0] = DataTypeHelper.GetDate(dr["PreviousLogin"]);
                    results[1] = DataTypeHelper.GetDate(dr["FailedLogin"]);
                    results[2] = DataTypeHelper.GetDate(dr["LastLogin"]);
                }
                return true;
            };

            DBManager2.Instance.ExecuteReader(dbSetting, func);

            return results;
        }

        public static int UpdateMemberActivity(string loginName, int memberSiteActivityID)
        {
            int result = 0;

            DBSettingEntity dbSetting = new DBSettingEntity(DataProvider.SqlServer, ConnectionStringManager.GetConnectionString(ConnectionStringEnum.DBSource.APP), CommandType.StoredProcedure, "[dbo].[Security_UpdateMemberActivity]", false);
            dbSetting.AddParameters("@LoginName", loginName, ParameterDirection.Input);
            dbSetting.AddParameters("@MemberSiteActivityID", memberSiteActivityID, ParameterDirection.Input);

            Func<IDataReader, bool> func = delegate (IDataReader dr)
            {
                if (dr.Read())
                {
                    result = Convert.ToInt32(dr["SsoOnlineUserId"]);
                }
                return true;
            };

            DBManager2.Instance.ExecuteReader(dbSetting, func);

            return result;
        }

        public static void KickoutMemberOnline(string tokenid)
        {
            DBSettingEntity dbSetting = new DBSettingEntity(DataProvider.SqlServer, ConnectionStringManager.GetConnectionString(ConnectionStringEnum.DBSource.APP), CommandType.StoredProcedure, "[dbo].[Security_KickoutMemberOnline]", false);
            dbSetting.AddParameters("@TokenID", tokenid, ParameterDirection.Input);

            DBManager2.Instance.ExecuteNonQuery(dbSetting);

        }
    }
}
