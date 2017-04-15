using System;

namespace APP.Infrastructure.SsoManagement.Common
{
    public class DataTypeHelper
    {
        public static string GetString(object dbstr)
        {
            if (dbstr != null && dbstr.GetType() != typeof(DBNull))
            {
                return (string)dbstr;
            }
            else
            {
                return string.Empty;
            }
        }

        public static DateTime GetDate(object dbdate)
        {
            if (dbdate != null && dbdate.GetType() != typeof(DBNull))
            {
                return (DateTime)dbdate;
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        public static int GetInt(object dbInt)
        {
            if (dbInt != null && dbInt.GetType() != typeof(DBNull))
            {
                return Convert.ToInt32(dbInt);
            }
            else
            {
                return 0;
            }
        }

        public static long GetLong(object dbLong)
        {
            if (dbLong != null && dbLong.GetType() != typeof(DBNull))
            {
                return Convert.ToInt64(dbLong);
            }
            else
            {
                return 0;
            }
        }

        public static bool GetBoolean(object dbBool)
        {
            if (dbBool != null && dbBool.GetType() != typeof(DBNull))
            {
                return Convert.ToBoolean(dbBool);
            }
            else
            {
                return false;
            }
        }
    }
}
