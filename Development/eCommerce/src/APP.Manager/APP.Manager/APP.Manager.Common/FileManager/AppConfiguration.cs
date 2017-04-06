using System.IO;
using APP.Common.Config;

namespace APP.Manager.Common.FileManager
{
    public class AppConfiguration : Configuration<AppConfiguration>
    {
        private AppConfiguration()
            : base(true)
        { }

        public const string ConfigFileName = "FileSetting.config";
        public FileSetting fileSetting { get; set; }

        public static void Initialize(string configFolder)
        {
            ConfigurationManager<AppConfiguration>.Register(configFolder);
        }

        public override void Load()
        {
            this.fileSetting = this.Deserialize<FileSetting>(ConfigFileName);

            if (!Directory.Exists(this.fileSetting.sourceRootPath))
            {
                Directory.CreateDirectory(this.fileSetting.sourceRootPath);
            }
            if (!Directory.Exists(this.fileSetting.destinationRootPath))
            {
                Directory.CreateDirectory(this.fileSetting.destinationRootPath);
            }
            if (!Directory.Exists(this.fileSetting.archiveRootPath))
            {
                Directory.CreateDirectory(this.fileSetting.archiveRootPath);
            }

        }

    }
}
