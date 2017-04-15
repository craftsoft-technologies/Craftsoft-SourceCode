
namespace APP.Infrastructure.SsoManagement.Common
{
    public enum SignInPolicyType
    {
        SingleSignIn,
        NormalSignIn
    }

    public enum SignOutPolicyType
    {
        SingleSignOut,
        NormalSignOut
    }

    public enum SessionStatus
    {
        NA = 0,
        Valid = 1,
        Expired = 2,
        NotExists = 3,
        Kickout = 4
    }
}
