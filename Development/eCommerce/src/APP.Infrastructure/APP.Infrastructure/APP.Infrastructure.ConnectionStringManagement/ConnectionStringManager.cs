using System;
using System.Transactions;

namespace APP.Infrastructure.ConnectionStringManagement
{
    public static class ConnectionStringManager
    {
        public static string GetConnectionString(ConnectionStringEnum.DBSource dbSource)
        {
            switch (dbSource)
            {
                case ConnectionStringEnum.DBSource.APP:
                    return AppConfiguration.Instance.conString.DB_APP;
                default:
                    return AppConfiguration.Instance.conString.DB_APP;
            }
        }

        public static string GetConnectionString(ConnectionStringEnum.DBSource dbSource, DateTime fromDate, DateTime toDate)
        {
            throw new NotImplementedException();
        }

        public static TransactionScope CreateReadCommitted()
        {
            var options = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TransactionManager.DefaultTimeout
            };

            return new TransactionScope(TransactionScopeOption.Required, options);
        }
    }
}
