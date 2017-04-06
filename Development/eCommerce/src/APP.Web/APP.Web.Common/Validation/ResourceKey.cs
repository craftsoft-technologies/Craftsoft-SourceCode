using System;
using System.Diagnostics;
using System.Resources;
using APP.Web.Common.Properties;

namespace APP.Web.Common.Validation
{
    public class ResourceKey
    {
        public const string FieldTwoDecimal = "FieldTwoDecimal";
        public const string FieldFourDecimal = "FieldFourDecimal";
        public const string FieldZeroDecimal = "FieldZeroDecimal";
        public const string Zero_OneBillion = "Int_Zero_OneBillion";
        public const string Int_One_OneBillion = "Int_One_OneBillion";
        public const string OneBillion_WithSign = "OneBillion_WithSign";
        public const string FieldDigits = "FieldDigits";
        public const string FieldRequired = "FieldRequired";
        public const string LoginName_Exist = "LoginName_Exist";
        public const string Length_Six_Twenty = "Length_Six_Twenty";
        public const string Length_Six_Eleven = "Length_Six_Eleven";
        public const string LoginName_Length = "LoginName_Length";
        public const string QuickLoginName_Length = "QuickLoginName_Length";
        public const string Length_ThreeHundred = "Length_ThreeHundred";
        public const string Length_TwoHundred = "Length_TwoHundred";
        public const string Length_OneHundred = "Length_OneHundred";
        public const string Length_OneHundredFifty = "Length_OneHundredFifty";
        public const string Length_Fifteen = "Length_Fifteen";
        public const string Length_Fifty = "Length_Fifty";
        public const string Length_Thirty = "Length_Thirty";
        public const string Length_Twenty = "Length_Twenty";
        public const string ForbiddenKeyword_Length = "ForbiddenKeyword_Length";
        public const string PostalCode = "PostalCode";
        public const string ContactNumber = "ContactNumber";
        public const string ContactNoExist = "ContactNoExist";
        public const string LoginName = "LoginName";
        public const string KeywordExists = "KeywordExists";
        public const string Email = "Email";
        public const string EmailExist = "EmailExist";
        public const string ConfirmPassword = "ConfirmPassword";
        public const string ConfirmEmail = "ConfirmEmail";
        public const string FourDigits = "FourDigits";
        public const string Please_Input_Captcha = "Please_Input_Captcha";
        public const string Please_Input_Password = "Please_Input_Password";
        public const string Please_Input_Loginname = "Please_Input_Loginname";
        public const string AccountNumber = "AccountNumber";
        public const string Url = "Url";
        public const string Invalid_Time = "Invalid_Time";
        public const string Invalid_Time_Without_Second = "Invalid_Time_Without_Second";
        public const string ConfirmCorrectOrder = "ConfirmCorrectOrder";

        [Conditional("DEBUG")]
        public static void ValidResources()
        {
            var validatioResourceManager = new ResourceManager(typeof(ValidationMessages));
            var fields = typeof(ResourceKey).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            foreach (var field in fields)
            {
                var resourceKey = field.GetValue(null).ToString();
                var resource = validatioResourceManager.GetString(resourceKey);
                if (resource == null)
                {
                    throw new Exception("ResourceKey:" + resourceKey + " can't be found in GCT.Web.Common.Properties.ValidationMessages.resx, please correct");
                }
            }
        }
    }
}
