using System;
using System.Runtime.Serialization;

namespace APP.Web.eCommerce.BLL.Model
{
    [Serializable]
    [DataContract]
    public class Postlogin
    {
        [DataMember(Name = "mn", EmitDefaultValue = false)]
        public string MemberName { get; set; }

        public DateTime? LastSuccessLogin { get; set; }

        [DataMember(Name = "lt", EmitDefaultValue = false)]
        public string LastSuccessLoginForJs
        {
            get
            {
                if (LastSuccessLogin == null)
                {
                    return null;
                }
                else
                {
                    return LastSuccessLogin.Value.ToString("yyyy,MM-1,dd,HH,mm,ss");//MM-1 is for JavaScript parse
                }
            }
            set
            {
                throw new NotSupportedException();
            }
        }


        [DataMember(Name = "gn")]
        public string GivenName { get; set; }

        [DataMember(Name = "ln")]
        public string LastName { get; set; }

        [DataMember(Name = "pl")]
        public string PreferedLanguage { get; set; }

    }
}
