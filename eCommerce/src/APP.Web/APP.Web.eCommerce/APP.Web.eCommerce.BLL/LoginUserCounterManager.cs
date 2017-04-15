using System;
using System.Collections.Generic;
using System.Linq;
using APP.Web.Common.Utilities;

namespace APP.Web.eCommerce.BLL
{
    public class LoginUserCounterManager
    {
        private static readonly object syncRoot = new object();
        private static Dictionary<string, DateTime> userDict = new Dictionary<string, DateTime>();

        public static int NoOfLoginUser
        {
            get { return userDict.Count; }
        }

        public static void AddLoginUser(string loginName, string tokenId, DateTime loginDateTime)
        {
            string key = loginName + "-" + tokenId;

            if (!userDict.ContainsKey(key))
            {
                lock (syncRoot)
                {
                    userDict.Add(key, loginDateTime);
                }

                LogHelper.User.Debug(string.Format("AddLoginUser [{0} {1}]", key, loginDateTime));
            }
            else
            {
                lock (syncRoot)
                {
                    userDict[key] = loginDateTime;
                }

                LogHelper.User.Debug(string.Format("AddLoginUser but ContainsKey [{0} {1}]. So update loginDateTime", key, loginDateTime));
            }
        }

        public static void RemoveLoginUser(string loginName, string tokenId)
        {
            string key = loginName + "-" + tokenId;

            if (userDict.ContainsKey(key))
            {
                lock (syncRoot)
                {
                    userDict.Remove(key);
                }

                LogHelper.User.Debug(string.Format("RemoveLoginUser [{0}]", key));
            }
            else
            {
                LogHelper.User.Debug(string.Format("RemoveLoginUser but does not ContainsKey [{0}]", key));
            }
        }

        public static Dictionary<string, DateTime> LoginUserList(string orderBy)
        {
            if (orderBy == "Ascending")
            {
                return (from entry in userDict orderby entry.Value ascending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
            }
            else
            {
                return (from entry in userDict orderby entry.Value descending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
            }
        }
    }
}
