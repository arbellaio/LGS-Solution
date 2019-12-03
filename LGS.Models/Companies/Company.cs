using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using LGS.Models.Communication;
using LGS.Models.Credits;
using LGS.Models.Users;

namespace LGS.Models.Companies
{
    public class Company
    {
        public int Id { get; set; }
        public string LogoPath { get; set; }
        public string CompanyName { get; set; }
        public string MainDescription { get; set; }

        [AllowHtml]
        public string ShortDescription { get; set; }

        [AllowHtml]
        public string LongDescription { get; set; }

        public float Ratings { get; set; }
        public string PhoneNumber { get; set; }
        public string AddressOneUnit{ get; set; }
        public string AddressTwoStreet { get; set; }
        public string AddressThreeLocality{ get; set; }
        public string PostalCode { get; set; }
        public string CompanyEmail { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsDeleted { get; set; }
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

        [AllowHtml]
        public string MapLocation { get; set; }

       

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreditUseDateTime { get; set; }

        public int DeliveryInterval { get; set; }
        public int NotificationMode { get; set; }
        public int LeadLimit { get; set; }

        public virtual List<CompanyInvoice> CompanyInvoices { get; set; }
        public virtual List<CompanyRating> CompanyRatings { get; set; }
        public virtual List<CompanyInventory> CompanyInventories { get; set; }

        public virtual List<CustomerReview> CustomerReviews { get; set; }
        public virtual List<CustomerMessage> CustomerMessages { get; set; }
    }
}
