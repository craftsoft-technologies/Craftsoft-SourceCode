using APP.Infrastructure.SsoManagement.Model;

namespace APP.Infrastructure.SsoManagement.Policies
{
    public interface ISignInPolicy
    {
        int WebsiteID { get; set; }
        SessionInfo Login(SessionInfo session);
    }
}
