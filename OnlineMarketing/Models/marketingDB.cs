using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineMarketing.Models
{
    public class marketingDB
    {
        [MetadataType(typeof(adminValidation))]
        public partial class admin
        {
        }

        public class adminValidation
        {
            [Key]
            public int ad_id { get; set; }

            [Display(Name ="User Name")]
            [Required(ErrorMessage ="*")]
            public string ad_username { get; set; }

            [Display(Name = "Password")]
            [Required(ErrorMessage = "*")]
            public string ad_password { get; set; }
            //public virtual ICollection<category> categories { get; set; }
        }


        [MetadataType(typeof(categoryValidation))]
        public partial class category
        {          
        }


        public class categoryValidation
        {
            [Key]
            public int cat_id { get; set; }

            [Display(Name = "Category Name")]
            [Required(ErrorMessage = "*")]
            public string cat_name { get; set; }

            [Display(Name = "Password ")]
            [Required(ErrorMessage = "*")]
            public string cat_image { get; set; }

            [Display(Name = "Admin")]
            [Required(ErrorMessage = "*")]
            public Nullable<int> cat_fk_ad { get; set; }
        }


        [MetadataType(typeof(productValidation))]
        public partial class product
        {
        }


        public  class productValidation
        {
            [Key]
            public int pro_id { get; set; }

            [Display(Name = "Product Name")]
            [Required(ErrorMessage = "*")]
            public string pro_name { get; set; }

            [Display(Name = "Image")]
            [Required(ErrorMessage = "*")]
            public string pro_image { get; set; }

            [Display(Name = "Description")]
            [Required(ErrorMessage = "*")]
            public string pro_description { get; set; }

            [Display(Name = "Price")]
            [Required(ErrorMessage = "*")]
            public Nullable<int> pro_price { get; set; }

            //[Display(Name = "")]
            //[Required(ErrorMessage = "*")]
            //public Nullable<int> pro_fk_category { get; set; }

            //[Display(Name = "")]
            //[Required(ErrorMessage = "*")]
            //public Nullable<int> pro_fk_users { get; set; }
        }


        public partial class user
        {
        }

        public partial class user
        {
            [Key]
            public int u_id { get; set; }

            [Display(Name = "User Name")]
            [Required(ErrorMessage = "*")]
            public string u_username { get; set; }

            [Display(Name = "Email")]
            [Required(ErrorMessage = "*")]
            public string u_email { get; set; }

            [Display(Name = "Password")]
            [Required(ErrorMessage = "*")]
            public string u_password { get; set; }

            [Display(Name = "Image")]
            [Required(ErrorMessage = "*")]
            public string u_image { get; set; }

            [Display(Name = "Contact Number")]
            [Required(ErrorMessage = "*")]
            public string u_contact { get; set; }
        }

    }
}