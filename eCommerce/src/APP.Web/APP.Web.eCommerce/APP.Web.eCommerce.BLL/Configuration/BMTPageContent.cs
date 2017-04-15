using System;
using System.Collections.Generic;

namespace APP.Web.eCommerce.BLL.Configuration
{
    public class BmtPageContent
    {
        public int PageId { get; set; }
        public string PageName { get; set; }
        public List<BannerSetting> Banners { get; set; }
    }

    public class BannerSetting
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Template { get; set; }
    }
}
