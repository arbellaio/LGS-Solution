using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LGS.Models.Users;

namespace LGS.Models.Credits
{
    // This class will be used to create invoice for sale/purchase
    // done with LGS its LGS / User record irrelevant of company or
    // account its like receipt in shopping mall. 
    public class CreditInvoice
    {
        public int Id { get; set; }

        public string InvoiceNo { get; set; }
        public string TransactionId { get; set; }
        public string TransactionService { get; set; }
        public decimal TransactionAmount { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SkuCode { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }
        public string TotalPrice { get; set; }
        public string Tax { get; set; }
        public string Url { get; set; }
        public string Currency { get; set; }



        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        public string UserId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime UpdatedDate { get; set; }


        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
        public int ClientId { get; set; }

    }
}
