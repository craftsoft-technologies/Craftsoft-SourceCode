using System;
using APP.Common.Exception;
using APP.Entities.Enum;

namespace APP.Manager.Common.Exception
{
    [Serializable]
    public class BackOfficeException : BaseException
    {
        public BackOfficeException(object error, ErrorEnum.ErrCode errorCode) : base(Convert.ToInt64(error).ToString(), errorCode.ToString()) { }
        public BackOfficeException(object error, System.Exception innerEx, ErrorEnum.ErrCode errorCode) : base(Convert.ToInt64(error).ToString(), innerEx, errorCode.ToString()) { }
        public BackOfficeException(string error, ErrorEnum.ErrCode errorCode) : base(error, errorCode.ToString()) { }
        public BackOfficeException(string error, System.Exception innerEx, ErrorEnum.ErrCode errorCode) : base(error, innerEx, errorCode.ToString()) { }
    }
}
