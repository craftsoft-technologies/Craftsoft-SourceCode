using System.Threading;

namespace APP.Manager.Common.Utilities
{
    public class SecurityUtils
    {
        public static string GetCurrentUser()
        {
            if (Thread.GetData(Thread.GetNamedDataSlot("LoginName")) != null)
                return Thread.GetData(Thread.GetNamedDataSlot("LoginName")).ToString();
            else
                return "HardCode_Security";
        }
    }
}
