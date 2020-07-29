using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMarketing.Models
{
    public class AdViewmodel
    {
        public int pro_id { get; set; }
        public string pro_name { get; set; }
        public string pro_image { get; set; }
        public string pro_description { get; set; }
        public Nullable<int> pro_price { get; set; }
        public Nullable<int> pro_fk_category { get; set; }
        public Nullable<int> pro_fk_users { get; set; }

        public int cat_id { get; set; }
        public string cat_name { get; set; }

        public string u_username { get; set; }
        public string u_image { get; set; }
        public string u_contact { get; set; }

    }
}