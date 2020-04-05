using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using WebApplication3.Models;

namespace JobOffers.Models
{
    public class Jobs
    {
        public int Id { get; set; }
        [DisplayName("اسم الوظيفة")]
        public string JobTitle { get; set; }
        [DisplayName("وصف الوظيفة")]
        public string JobContent { get; set; }
        [DisplayName("صورة الوظيفة")]
        public string JobImage { get; set; }
        [DisplayName("نوع الوظيفة")]
        public int CategoryID { get; set; }

        public string UserID { get; set; }
        //one to many
        public virtual Category Category { get; set; }
        public virtual ApplicationUser user { get; set; }
    }
}