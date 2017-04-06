using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Infrastructure.EmailManagement.Configuration
{
    public class EmailSettingEntity
    {
        public string SmtpServer { get; set; }
        public int SmtpServerPort { get; set; }
        public string SmtpServerUsername { get; set; }
        public string SmtpServerPassword { get; set; }
        public bool SmtpEnableSSL { get; set; }

        public string SenderMailAddress { get; set; }
        public string SenderNoReplyMailAddress { get; set; }
        public string SenderContact { get; set; }

        public string EmailImageFolderUrl { get; set; }
    }
}
