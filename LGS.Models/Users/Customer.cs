using System;
using System.ComponentModel.DataAnnotations;

namespace LGS.Models.Users
{
    public class Customer
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime UpdatedDate { get; set; }

    }
}