using APP.Common.Config;

namespace APP.Infrastructure.ConnectionStringManagement
{
    public class AppConfiguration : APP.Common.Config.Configuration<AppConfiguration>
    {
        private AppConfiguration()
            : base(true)
        { }

        public const string ConfigFileName = "DBConnectionString.config";
        public ConnectionString conString { get; set; }

        public override void Load()
        {
            this.conString = this.Deserialize<ConnectionString>(ConfigFileName);
        }

        public static void Initialize(string configFolder)
        {
            ConfigurationManager<AppConfiguration>.Register(configFolder);
        }

    }
}
