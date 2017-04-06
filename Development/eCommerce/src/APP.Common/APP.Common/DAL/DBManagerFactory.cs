using System;
using System.Data;
using System.Data.SqlClient;
using Npgsql;

namespace APP.Common.DAL
{
    public class DBManagerFactory
    {
        private DBManagerFactory() { }

        public static IDbConnection GetConnection(DataProvider providerType)
        {
            IDbConnection iDbConnection = null;
            switch (providerType)
            {
                case DataProvider.SqlServer:
                    iDbConnection = new SqlConnection();
                    break;
                case DataProvider.Npgsql:
                    iDbConnection = new NpgsqlConnection();
                    break;
                default:
                    return null;
            }
            return iDbConnection;
        }

        public static IDbCommand GetCommand(DataProvider providerType)
        {
            switch (providerType)
            {
                case DataProvider.SqlServer:
                    return new SqlCommand();
                case DataProvider.Npgsql:
                    return new NpgsqlCommand();
                default:
                    return null;
            }
        }

        public static IDbDataAdapter GetDataAdapter(DataProvider providerType)
        {
            switch (providerType)
            {
                case DataProvider.SqlServer:
                    return new SqlDataAdapter();
                case DataProvider.Npgsql:
                  return new NpgsqlDataAdapter();
                default:
                    return null;
            }
        }

        public static IDbDataParameter GetParameter(DataProvider providerType)
        {
            IDbDataParameter iDataParameter = null;
            switch (providerType)
            {
                case DataProvider.SqlServer:
                    iDataParameter = new SqlParameter();
                    break;
                case DataProvider.Npgsql:
                    iDataParameter = new NpgsqlParameter();
                    break;
            }
            return iDataParameter;
        }

        public static IDbDataParameter[] GetParameters(DataProvider providerType, int paramsCount)
        {
            IDbDataParameter[] idbParams = new IDbDataParameter[paramsCount];

            switch (providerType)
            {
                case DataProvider.SqlServer:
                    for (int i = 0; i < paramsCount; ++i)
                    {
                        idbParams[i] = new SqlParameter();
                    }
                    break;
                case DataProvider.Npgsql:
                    for (int i = 0; i < paramsCount; ++i)
                    {
                        idbParams[i] = new NpgsqlParameter();
                    }
                    break;
                default:
                    idbParams = null;
                    break;
            }
            return idbParams;
        }

        public static IDbConnection BuilConnection(DataProvider providerType, string connectionString)
        {
            switch (providerType)
            {
                case DataProvider.SqlServer:
                    return new SqlConnection(connectionString);
                case DataProvider.Npgsql:
                    return new NpgsqlConnection(connectionString);
                default:
                    return null;
            }
        }

        public static IDbCommand BuildCommand(DataProvider providerType, IDbConnection connection, IDbTransaction transaction, DBSettingEntity dbSetting)
        {
            IDbCommand idbCommand = null;
            switch (providerType)
            {
                case DataProvider.SqlServer:
                    idbCommand = new SqlCommand();
                    break;
                case DataProvider.Npgsql:
                    idbCommand = new NpgsqlCommand();
                    break;
                default:
                    return null;
            }

            idbCommand.Connection = connection;
            idbCommand.CommandText = dbSetting.commandText;
            idbCommand.CommandType = dbSetting.commandType;
            idbCommand.CommandTimeout = dbSetting.commandTimeOut;

            if (transaction != null)
            {
                idbCommand.Transaction = transaction;
            }

            return idbCommand;

        }

        public static IDbDataParameter BuildParameter(DataProvider providerType, string paramName, object paramValue, SqlDbType? sqlDbType = null, int? dataTypeLength = null)
        {
            IDbDataParameter idbParameter = null;
            switch (providerType)
            {
                case DataProvider.SqlServer:
                    idbParameter = new SqlParameter();
                    break;
                case DataProvider.Npgsql:
                    idbParameter = new NpgsqlParameter();
                    break;
            }

            idbParameter.ParameterName = paramName;
            idbParameter.Value = paramValue;

            if ((idbParameter.Direction == ParameterDirection.InputOutput) && (idbParameter.Value == null))
            {
                idbParameter.Value = DBNull.Value;
            }

            if (sqlDbType.HasValue)
            {
                SqlParameter sqlParam = new SqlParameter();
                sqlParam.SqlDbType = sqlDbType.Value;
                idbParameter.DbType = sqlParam.DbType;
            }

            if (dataTypeLength.HasValue)
            {
                idbParameter.Size = dataTypeLength.Value;
            }

            return idbParameter;
        }
    }
}
