﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LGS.Models.Credits
{
    public class CreditInvoice
    {
        public int Id { get; set; }

        public string InvoiceNo { get; set; }
        public string TransactionId { get; set; }
        public string AccountId { get; set; }
        public string TransactionService { get; set; }
        public decimal TransactionAmount { get; set; }
        public int CompanyId { get; set; }


        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        public string UserId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime UpdatedDate { get; set; }

    }
}