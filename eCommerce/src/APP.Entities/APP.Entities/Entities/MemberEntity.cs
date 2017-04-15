using System;
using System.Collections.Generic;
using APP.Entities.Enum;

namespace APP.Entities.Entities
{
    [Serializable]
    public class MemberEntity
    {

        #region GeneralInformation

        public int memberId { get; set; }
        public string memberName { get; set; }
        public string salutation { get; set; }
        public string givenName { get; set; }
        public string lastName { get; set; }
        public MemberEnum.Gender gender { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string postalCode { get; set; }
        public string countryCode { get; set; }
        public string email { get; set; }
        public string contactNo { get; set; }
        public MemberEnum.ContactStatus contactStatus { get; set; }
        public string timeZone { get; set; }
        public MemberEnum.MemberStatus memberStatus { get; set; }
        public bool isDelete { get; set; }
        public int? affiliateId { get; set; }
        public int? selfAffiliateId { get; set; }
        public string preferedLanguage { get; set; }
        public string securityQuestion { get; set; }
        public string securityAnswer { get; set; }
        public string securityNumber { get; set; }
        public bool newsletterSubsription { get; set; }
        public bool phoneCallSubscription { get; set; }
        public int? newsletterSubscriptionReasonID { get; set; }
        public int? phoneCallSubscriptionReasonID { get; set; }
        public string promotionCode { get; set; }
        public MemberEnum.accounttype AccountType { get; set; }
        public string userCreated { get; set; }
        public string userUpdated { get; set; }
        public DateTime? dateUpdated { get; set; }
        public DateTime dateCreated { get; set; }
        public string currencyCode { get; set; }
        public int bankAccountGroupID { get; set; }
        public string qqID { get; set; }
        public int wingAccountId { get; set; }

        public string bankAccountGroup { get; set; }
        public string WingAccountNo { get; set; }
        public string affiliate { get; set; }
        public string referralCode { get; set; }
        public int memberLevel { get; set; }
        public string partnerAffiliateCode { get; set; }
        public bool isFirstTimeMigration { get; set; }
        public bool isMigratedMember { get; set; }

        public int? depositLimit { get; set; }
        public int? withdrawalLimit { get; set; }

        public string ipAddress { get; set; }
        public string ipServer { get; set; }
        public DateTime loginTime { get; set; }
        public string elapsedTime { get; set; }
        public int chanel { get; set; }
        public string lastActivity { get; set; }
        public string wechatID { get; set; }
        public string receiverName { get; set; }
        public string receiverNo { get; set; }
        public string province { get; set; }
        public int registeredChannel { get; set; }
        public string deliveryAddress { get; set; }
        public string deliveryCity { get; set; }
        public string deliveryProvince { get; set; }
        public string deliveryPostalCode { get; set; }

        #endregion

        #region Website Info

        public int websiteId { get; set; }
        public string websiteName { get; set; }

        #endregion

        public MemberLoginEntity memberLogin { get; set; }
    }
}
