using System;
using System.Collections.Generic;

namespace APP.Web.eCommerce.BLL.Configuration
{
    public class CountryDefaultSettings : List<CountryDefaultSetting>
    {
        private bool hasInit;
        private readonly object syncRoot = new object();
        private Dictionary<string, CountryDefaultSetting> cache;
        public CountryDefaultSetting Default { get; set; }

        private void TryInit()
        {
            if (hasInit) return;
            lock (syncRoot)
            {
                if (hasInit) return;
                var newCache = new Dictionary<string, CountryDefaultSetting>();
                foreach (var countryDefaultSetting in this)
                {
                    if (countryDefaultSetting.CountryCode == string.Empty)
                    {
                        Default = countryDefaultSetting;
                    }
                    else
                    {
                        newCache.Add(countryDefaultSetting.CountryCode, countryDefaultSetting);
                    }
                }
                cache = newCache;
                hasInit = true;
            }
        }

        public CountryDefaultSetting GetSetting(string countryCode)
        {
            TryInit();
            CountryDefaultSetting countryDefaultSetting;
            if (cache.TryGetValue(countryCode, out countryDefaultSetting))
            {
                return countryDefaultSetting;
            }
            return Default;
        }
    }
}
