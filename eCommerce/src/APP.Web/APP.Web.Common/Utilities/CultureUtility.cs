using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Web.Common.Utilities
{
    public static class CultureUtility
    {
        public static string GetCultureLowerName(string languageCode)
        {
            switch (languageCode)
            {
                case "ENG":
                    return "en";

                case "IDN":
                    return "id";

                default:
                    throw new ArgumentOutOfRangeException("languageCode");
            }
        }
    }
}
