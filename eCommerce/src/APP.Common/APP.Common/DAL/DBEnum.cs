using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Common.DAL
{
    public enum DataProvider
    {
        SqlServer, Npgsql, OleDb, Odbc
    }

    public enum DBError
    {
        /// <summary>800000001 - Error On execute Stored Proc</summary>
        DATA_ACCESS_ERROR = 800000001,
        /// <summary>800000002 - Connection Not Created</summary>
        NO_CONNECTION = 800000002,
        /// <summary>800000003 - Not Supported Data Provider</summary>
        NOT_SUPPORTED_DATA_PROVIDER = 800000003,
        /// <summary>800000004 - Reference data not found</summary>
        DATA_REFERENCE_NOT_FOUND = 800000004,
        /// <summary>800000005 - Data duplicate</summary>
        DATA_DUPLICATED = 800000005,
    }
}
