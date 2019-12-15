using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using LGS.Models.Companies;
using LGS.Models.Users;

namespace LGS.Models.Communication
{
    public class CustomerMessage
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string CustomerFullName { get; set; }

        public string AddressOneUnit { get; set; }
        public string AddressTwoStreet { get; set; }
        public string AddressThreeLocality { get; set; }
        public string PostalCode { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }
      
        public int CompanyId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime UpdatedDate { get; set; }

        [NotMapped]
        [AllowHtml]
        public string MapLocation { get; set; }
    }

    public class EmailComparer : IEqualityComparer<CustomerReview>
    {
        public bool Equals(CustomerReview x, CustomerReview y)
        {
            if (string.Equals(x.CustomerEmail, y.CustomerEmail, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }

        public int GetHashCode(CustomerReview obj)
        {
            return obj.CustomerEmail.GetHashCode();
        }
    }
}
