using System;

namespace APP.Manager.Common.FileManager
{
    [Serializable]
    public class FileSetting
    {
        public string sourceRootPath { get; set; }
        public string destinationRootPath { get; set; }
        public string archiveRootPath { get; set; }
    }
}
