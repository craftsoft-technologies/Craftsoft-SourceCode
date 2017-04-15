using System;

namespace APP.Entities.Entities
{
    public class GeneralSettingEntity
    {
        public int generalSettingId { get; set; }
        public string description { get; set; }
        public string value { get; set; }
        public int orderNo { get; set; }

        public string userCreated { get; set; }
        public string userUpdated { get; set; }
        public DateTime? dateUpdated { get; set; }
        public DateTime dateCreated { get; set; }

    }
}
