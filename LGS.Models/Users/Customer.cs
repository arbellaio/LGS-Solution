using System;
using System.ComponentModel.DataAnnotations;

namespace LGS.Models.Users
{
    public class Customer
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string AddressOneUnit { get; set; }
        public string AddressTwoStreet { get; set; }
        public string AddressThreeLocality { get; set; }
        public string PostalCode { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime UpdatedDate { get; set; }

    }
}