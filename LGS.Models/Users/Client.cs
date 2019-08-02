using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LGS.Models.Companies;

namespace LGS.Models.Users
{
    public class Client
    {
        public int Id { get; set; }

        [ForeignKey("AppUserId")]
        public virtual ApplicationUser User { get; set; }
        public string AppUserId { get; set; }
        public string PhoneNumber { get; set; }
        public string FacebookPageLink { get; set; }
        public string ProfilePhoto { get; set; }

        //        public string FacebookId { get; set; }
        //        public string GoogleId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime UpdatedDate { get; set; }

        public virtual List<Company> Companies { get; set; }

    }
}
