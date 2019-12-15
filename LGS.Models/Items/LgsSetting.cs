using System;
using System.ComponentModel.DataAnnotations;

namespace LGS.Models.Items
{
    public class LgsSetting
    {
        public int Id { get; set; }
        public decimal LeadsPerCredit { get; set; }
        public decimal CreditPerMoney { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        
    }
}