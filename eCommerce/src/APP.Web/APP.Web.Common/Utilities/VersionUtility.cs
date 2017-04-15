
namespace APP.Web.Common.Utilities
{
    public class VersionUtility
    {
        public int VersionNumber { get; set; }

        public int IncreaseVersion()
        {
            if (VersionNumber == int.MaxValue)
            {
                VersionNumber = 1;
            }
            VersionNumber += 1;
            return VersionNumber;
        }

        public bool NeedRefresh(int versionNumber)
        {
            return versionNumber != VersionNumber;
        }
    }
}
