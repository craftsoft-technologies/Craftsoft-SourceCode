using System;
using APP.Common.Exception;
using APP.Entities.Enum;

namespace APP.Manager.Common.Exception
{
    [Serializable]
    public class MemberException : BaseException
    {
        public MemberException(object error, ErrorEnum.ErrCode errorCode) : base(Convert.ToInt64(error).ToString(), errorCode.ToString()) { }
        public MemberException(object error, System.Exception innerEx, ErrorEnum.ErrCode errorCode) : base(Convert.ToInt64(error).ToString(), innerEx, errorCode.ToString()) { }
        public MemberException(string error, ErrorEnum.ErrCode errorCode) : base(error, errorCode.ToString()) { }
        public MemberException(string error, System.Exception innerEx, ErrorEnum.ErrCode errorCode) : base(error, innerEx, errorCode.ToString()) { }
    }
}
