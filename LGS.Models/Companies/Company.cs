using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LGS.Models.Users;

namespace LGS.Models.Companies
{
    public class Company
    {
        public int Id { get; set; }
        public string LogoPath { get; set; }
        public string CompanyName { get; set; }
        public string MainDescription { get; set; }
        public string LongDescription { get; set; }
        public int Ratings { get; set; }

        public string FacebookId { get; set; }
        public string FacebookPageLink { get; set; }
        public string GoogleId { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
        public int ClientId { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime UpdatedDate { get; set; }
    }
}
