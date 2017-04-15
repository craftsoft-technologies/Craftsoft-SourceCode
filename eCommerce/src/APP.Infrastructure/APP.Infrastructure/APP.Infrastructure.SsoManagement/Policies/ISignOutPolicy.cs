
namespace APP.Infrastructure.SsoManagement.Policies
{
    public interface ISignOutPolicy
    {
        int WebsiteID { get; set; }
        void Logout(string TokenID);
    }
}
