
namespace APP.Infrastructure.EmailManagement.EmailTemplate
{
    public class EmailForgotPasswordContentEntity
    {
        public string MemberName { get; set; }
        public string WebsiteName { get; set; }
        public string NewPassword { get; set; }
        public string LanguageCode { get; set; }
        public string RecieverEmail { get; set; }
    }
}
