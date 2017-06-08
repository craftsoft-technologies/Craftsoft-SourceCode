using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace APP.Common.DAL
{
    public class DBManager2
    {
        private static DBManager2 instance = new DBManager2();

        public static DBManager2 Instance
        {
            get { return instance; }
        }

        private void AttachParameters(IDbCommand command, List<DBParameterEntity> lstParams, DataProvider providerType)
        {
            foreach (DBParameterEntity item in lstParams)
            {
                if (item.sqlDbType.HasValue)
                {
                    command.Parameters.Add(DBManagerFactory.BuildParameter(providerType, item.parameterName, item.parameterValue, item.sqlDbType, item.dataTypeLength));
                }
                else
                {
                    command.Parameters.Add(DBManagerFactory.BuildParameter(providerType, item.parameterName, item.parameterValue));
                }
            }
        }

        public int ExecuteNonQuery(DBSettingEntity dbSetting)
        {
            try
            {
                int result;
                using (IDbConnection con = DBManagerFactory.BuilConnection(dbSetting.providerType, dbSetting.connectionString))
                {
                    con.Open();
                    IDbTransaction idbTransaction = null;
                    if (dbSetting.IsTransaction) idbTransaction = con.BeginTransaction();

                    using (IDbCommand cmd = DBManagerFactory.BuildCommand(dbSetting.providerType, con, idbTransaction, dbSetting))
                    {

                        AttachParameters(cmd, dbSetting.parameters, dbSetting.providerType);

                        result = cmd.ExecuteNonQuery();

                        if (idbTransaction != null) idbTransaction.Commit();
                    }
                }

                return result;
            }
            catch (SqlException ex)
            {
                throw TranslateException(ex);
            }
        }

        public void ExecuteReader(DBSettingEntity dbSetting, Func<IDataReader, bool> funcProccessDataReader)
        {
            try
            {
                using (IDbConnection con = DBManagerFactory.BuilConnection(dbSetting.providerType, dbSetting.connectionString))
                {
                    con.Open();
                    IDbTransaction idbTransaction = null;
                    if (dbSetting.IsTransaction) idbTransaction = con.BeginTransaction();

                    using (IDbCommand cmd = DBManagerFactory.BuildCommand(dbSetting.providerType, con, idbTransaction, dbSetting))
                    {
                        AttachParameters(cmd, dbSetting.parameters, dbSetting.providerType);

                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            funcProccessDataReader.Invoke(dr);
                        }

                        if (idbTransaction != null) idbTransaction.Commit();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw TranslateException(ex);
            }
        }

        public object ExecuteScalar(DBSettingEntity dbSetting)
        {
            try
            {
                object result;
                using (IDbConnection con = DBManagerFactory.BuilConnection(dbSetting.providerType, dbSetting.connectionString))
                {
                    con.Open();
                    IDbTransaction idbTransaction = null;
                    if (dbSetting.IsTransaction) idbTransaction = con.BeginTransaction();

                    using (IDbCommand cmd = DBManagerFactory.BuildCommand(dbSetting.providerType, con, idbTransaction, dbSetting))
                    {
                        AttachParameters(cmd, dbSetting.parameters, dbSetting.providerType);

                        result = cmd.ExecuteScalar();

                        if (idbTransaction != null) idbTransaction.Commit();
                    }
                }

                return result;
            }
            catch (SqlException ex)
            {
                throw TranslateException(ex);
            }
        }

        public void ExecuteDataSet(DBSettingEntity dbSetting, Func<DataSet, bool> funcProccessDataSet)
        {
            try
            {
                using (IDbConnection con = DBManagerFactory.BuilConnection(dbSetting.providerType, dbSetting.connectionString))
                {
                    con.Open();
                    IDbTransaction idbTransaction = null;
                    if (dbSetting.IsTransaction) idbTransaction = con.BeginTransaction();

                    using (IDbCommand cmd = DBManagerFactory.BuildCommand(dbSetting.providerType, con, idbTransaction, dbSetting))
                    {
                        AttachParameters(cmd, dbSetting.parameters, dbSetting.providerType);

                        IDbDataAdapter dataAdapter = DBManagerFactory.GetDataAdapter(dbSetting.providerType);
                        dataAdapter.SelectCommand = cmd;
                        DataSet dataSet = new DataSet();
                        dataAdapter.Fill(dataSet);
                        funcProccessDataSet.Invoke(dataSet);

                        if (idbTransaction != null) idbTransaction.Commit();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw TranslateException(ex);
            }
        }
        
        #region Private Method
        public DBException TranslateException(SqlException ex)
        {      
            Logger.FileLogger.Instance.Exception(ex);

            Logger.FileLogger.Instance.Info("APP.Common - DAL - Translate Exception");
            Logger.FileLogger.Instance.Info("Exception Number: " + ex.Number.ToString());
            Logger.FileLogger.Instance.Info("Exception Message: " + ex.Message);
            if (ex.InnerException != null)
            {
                Logger.FileLogger.Instance.Info("Exception Inner Exception: " + ex.InnerException.Message);
            }

            switch (ex.Number)
            {
                case 547:
                    // ForeignKey Violation 
                    return new DBException(ex.Message, ex, DBError.DATA_REFERENCE_NOT_FOUND);
                case 2627:
                case 2601:
                    // Unique Index/Constriant Violation 
                    return new DBException(ex.Message, ex, DBError.DATA_DUPLICATED);
                default:
                    // throw a general DAL Exception 
                    return new DBException(ex.Message, ex, DBError.DATA_ACCESS_ERROR);
            }
        }
        #endregion
        
    }
}
