using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;
using APP.Web.Common.Utilities;
//using APP.Web.eCommerce.Website.App_GlobalResources;

namespace APP.Web.eCommerce.Website
{
    public class PartnerLocalizationManager
    {
        public static void Initialize(string partnerNamespace)
        {
            var baseResourceTypes = new[] { typeof(string), typeof(string) }; //{ typeof(Text), typeof(Message) };
            foreach (var baseResourceType in baseResourceTypes)
            {
                string partnerResourceTypeName = string.Format("APP.Web.eCommerce.Website._Partners.{0}.App_GlobalResources.{1}", partnerNamespace, baseResourceType.Name);
                Type partnerResourceType = Type.GetType(partnerResourceTypeName);
                if (partnerResourceType != null)
                {
                    LogHelper.Server.InfoFormat(" partnerResourceType loaded:{0}", partnerResourceTypeName);
                    var partnerLocalizationNames = GetLocalizationNames(partnerResourceType);
                    LogHelper.Server.InfoFormat(" Partner Localization Names:{0}", string.Join(",", partnerLocalizationNames));
                    var partnerResourceManagerProperty = partnerResourceType.GetProperty("ResourceManager", BindingFlags.Static | BindingFlags.Public);
                    var partnerResourceManager = (ResourceManager)partnerResourceManagerProperty.GetValue(null, null);

                    var baseResourceManagerField = baseResourceType.GetField("resourceMan", BindingFlags.Static | BindingFlags.NonPublic);
                    baseResourceManagerField.SetValue(null, new PartnerResourceManager(baseResourceType.FullName, baseResourceType.Assembly, partnerResourceManager, partnerLocalizationNames));
                }
                else
                {
                    LogHelper.Server.InfoFormat(" partnerResourceType not exist:{0}", partnerResourceTypeName);
                }
            }
        }

        private static List<string> GetLocalizationNames(IReflect resourcesType)
        {
            var localizationNames = new List<string>();
            foreach (var property in resourcesType.GetProperties(BindingFlags.Static | BindingFlags.Public))
            {
                if (property.PropertyType.Name == "String")
                {
                    localizationNames.Add(property.Name);
                }
            }
            return localizationNames;
        }
    }
}