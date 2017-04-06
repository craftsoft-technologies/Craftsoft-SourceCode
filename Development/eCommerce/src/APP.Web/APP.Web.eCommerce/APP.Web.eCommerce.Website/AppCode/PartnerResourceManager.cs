using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;

namespace APP.Web.eCommerce.Website
{
    public class PartnerResourceManager : ResourceManager
    {
        private readonly Dictionary<string, bool> partnerOverrided = new Dictionary<string, bool>();
        private readonly ResourceManager partnerResourceManager;

        public PartnerResourceManager(string baseName, Assembly assembly, ResourceManager partnerResourceManager, IEnumerable<string> partnerLocalizationNames)
            : base(baseName, assembly)
        {
            if (partnerResourceManager.ToString() != "System.Resources.ResourceManager")
            {
                throw new NotSupportedException("partnerResourceManager type must be System.Resources.ResourceManager");
            }
            this.partnerResourceManager = partnerResourceManager;
            partnerOverrided = new Dictionary<string, bool>();
            foreach (var partnerLocalizationName in partnerLocalizationNames)
            {
                partnerOverrided.Add(partnerLocalizationName, true);
            }
        }

        public override string GetString(string name, System.Globalization.CultureInfo culture)
        {
            if (partnerOverrided.ContainsKey(name))
            {
                return partnerResourceManager.GetString(name, culture);
            }
            return base.GetString(name, culture);
        }
    }
}