using System;
using APP.Common.Exception;
using APP.Entities.Enum;

namespace APP.Manager.Common.Exception
{
    [Serializable]
    public class PaymentException : BaseException
    {
        public PaymentException(object error, ErrorEnum.ErrCode errorCode) : base(Convert.ToInt64(error).ToString(), errorCode.ToString()) { }
        public PaymentException(object error, System.Exception innerEx, ErrorEnum.ErrCode errorCode) : base(Convert.ToInt64(error).ToString(), innerEx, errorCode.ToString()) { }
        public PaymentException(string error, ErrorEnum.ErrCode errorCode) : base(error, errorCode.ToString()) { }
        public PaymentException(string error, System.Exception innerEx, ErrorEnum.ErrCode errorCode) : base(error, innerEx, errorCode.ToString()) { }
        public PaymentException(object error, ErrorEnum.ErrCode errorCode, bool isMultiplerError) : base(error, errorCode.ToString(), isMultiplerError) { }
    }
}
