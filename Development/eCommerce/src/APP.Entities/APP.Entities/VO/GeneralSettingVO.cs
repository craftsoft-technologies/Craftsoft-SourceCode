using System;

namespace APP.Entities.VO
{
    public class GeneralSettingVO
    {
        public int generalSettingId { get; set; }
        public string value { get; set; }
    }

    public class CDNSetting
    {
        public string CDNSettingCode { get; set; }
        public bool IsUseExternalCDN { get; set; }
        public string CdnExternalUrl { get; set; }
    }
}
