using APP.Common.Config;

namespace APP.Infrastructure.EmailManagement.Configuration
{
    public class AppConfiguration : Configuration<AppConfiguration>
    {
        private AppConfiguration() : base(true) { }

        public const string ConfigFileName = "EmailConfiguration.config";
        public EmailSettingEntity mailSetting { get; set; }

        public override void Load()
        {
            this.mailSetting = this.Deserialize<EmailSettingEntity>(ConfigFileName);
        }

        public static void Initialize(string configFolder)
        {
            ConfigurationManager<AppConfiguration>.Register(configFolder);
        }
    }
}
