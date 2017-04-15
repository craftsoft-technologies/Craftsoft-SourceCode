using System.IO;
using APP.Common.Config;
using APP.Infrastructure.SsoManagement.Model;

namespace APP.Infrastructure.SsoManagement.Configuration
{
    public class AppConfiguration : Configuration<AppConfiguration>
    {
        private AppConfiguration() : base(true) { }

        public const string ConfigFileName = "SsoConfiguration.config";
        public SsoSetting ssoSetting { get; set; }

        public override void Load()
        {
            this.ssoSetting = this.Deserialize<SsoSetting>(Path.Combine(this.ConfigurationFolder, ConfigFileName));
        }

        public static void Initialize(string configFolder)
        {
            ConfigurationManager<AppConfiguration>.Register(configFolder);
        }
    }
}
