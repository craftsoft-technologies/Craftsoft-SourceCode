using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Entities.Enum
{
    public class ErrorEnum
    {
        // *********Description Error Code*******
        // 1st Digit : Severity Level
        //      1 - Show stopper
        //      2 - High, no workaround
        //      3 - Medium, with workaround
        //      4 - Low, Cosmetic, Enhancement     

        // 2nd Digit - Layering     
        //      1 - Web Layer            2 - Service Layer
        //      3 - Manager Layer        4 - Data Access Layer

        // 3rd & 4th Digit - Function Module     
        //      01 - Account        02 - Member         03 - Payment
        //      04 - Integration    05 - Document       06 - Currency 
        //      07 - Maintenance    08 - BackOffice     09 - Affiliate
        //      00 - General

        // 5th-8th Digit - Scenario No  
        public enum ErrCode
        {
            #region 00 General

            General = 4100001,
            GeneralInvalidParameter = 4100002,
            DATA_DUPLICATED_LOGINNAME = 4100003,

            #endregion

            #region 01 Account
            /// <summary>23010001 - MemberAccount not found</summary>
            MEMBERACCOUNT_NOT_FOUND = 23010001,
            /// <summary>23010002 - Invalid Status</summary>
            MEMBERACCOUNT_INVALID_STATUS = 23010002,
            #endregion

            #region 02 Member
            /// <summary>13020001 - Member not found</summary>
            MEMBER_NOT_FOUND = 23021001,
            /// <summary>23020002 - Member Inactive Member not allow login</summary>
            MEMBER_INVALID_LOGIN_INACTIVE_MEMBER = 23021002,
            /// <summary>23020003 - Password Lock</summary>
            MEMBER_INVALID_LOGIN_PWD_ISLOCKED = 23021003,
            /// <summary>23020004 - Invalid Password</summary>
            MEMBER_INVALID_PASSWORD_NOT_MATCH = 23021004,
            /// <summary>23020005 - Invalid Password</summary>
            MEMBER_INVALID_CAPTCHA = 23021005,
            /// <summary>23020006 - Invalid Email</summary>
            MEMBER_LOGINNAME_OR_EMAIL_NOT_MATCH = 23021006,
            /// <summary>23020007 - Wrong Security Answer or Security Number</summary>
            MEMBER_INVALID_SECURITY_INFO = 23021007,
            /// <summary>23020008 - Invalid Password Or Username</summary>
            MEMBER_INVALID_PASSWORD_OR_USERNAME = 23021008,
            /// <summary>23021009 Captcha expire</summary>
            MEMBER_CAPTCHA_EXPIRE = 23021009,
            /// <summary>23021013 Unable to clear, there is member tagged by this account tag</summary>
            MEMBER_UNABLE_DELETE_ACCOUNT_TAG = 23021013,
            /// <summary>23021014 - Please contact online customer service </summary>
            MEMBER_CONTACT_CUSTOMERS_SERVICE = 23021014,
            /// <summary>23021015 - Invalid Member Birthday or Underage </summary>
            MEMBER_INVALID_BIRTHDAY = 23021015,
            /// <summary>23021016 - Login failure for third times or more</summary>
            MEMBER_LOGIN_FAILURE_CONTACT_CS = 23021016,
            /// <summary>23021017 - Contact Already Exists</summary>
            CONTACT_NUMBER_DUPLICATE = 23021017,
            /// <summary>23021018 - Email Already Exists</summary>
            EMAIL_EXIST = 23021018,
            #endregion         

            #region 03 Payment
            /// <summary>43030001 - Payment Invalid Status</summary>
            PAYMENT_INVALID_STATUS = 43030001,
            #endregion

            #region 04 Document
            /// <summary>43051001 - Document not found</summary>
            DOCUMENT_NOT_FOUND = 43051001,
            /// <summary>43051003 - Document Information Not correct</summary>
            DOCUMENT_INVALID_INFO = 43051002,

            #endregion

            #region 05 Currency
            /// <summary>43060001 - Effective Date Earlier Than Today Day</summary>
            CURRENCY_EFFECTIVEDATE_EARLIER_TODAY = 43060001,
            /// <summary>43060002 - Currency Not Found</summary>
            CURRENCY_NOT_FOUND = 43060002,
            #endregion

            #region 07 Maintenance
            /// <summary>23071001 -Maintenance record not found</summary>
            MAINTENANCE_NOT_FOUND = 23071001,
            /// <summary>23071002 - Maintenance product not found</summary>
            MAINTENANCE_PRODUCT_TYPE_NOT_FOUND = 23071002,

            #endregion

            #region 08 BackOffice
            /// <summary>42080001 -user record duplicate</summary>
            USER_RECORD_DUPLICATE = 42080001,
            /// <summary>42080002 -role record duplicate</summary>
            ROLE_RECORD_DUPLICATE = 42080002,
            /// <summary>42080003 -bank record duplicate</summary>
            BANK_RECORD_DUPLICATE = 42080003,
            /// <summary>42080004 -whitelist record duplicate</summary>
            WHITELIST_RECORD_DUPLICATE = 42080004,
            /// <summary>42080004 -bank account group record duplicate</summary>
            BANK_ACCOUNT_GROUP_RECORD_DUPLICATE = 42080005,
            /// <summary>42080007 -user id/name or email not match with db</summary>
            USER_ID_OR_EMAIL_NOT_MATCH = 42080007,
            /// <summary>42080008 -user email not match with db</summary>
            USER_EMAIL_NOT_MATCH = 42080008,
            /// <summary>42080009 -user status inactive/// </summary>
            USER_INACTIVE = 42080009,
            #endregion
        }
    }
}
