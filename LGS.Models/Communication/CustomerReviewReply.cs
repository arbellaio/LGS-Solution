using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LGS.Models.Communication
{
    public class CustomerReviewReply
    {
        public int Id { get; set; }
        public string ReviewReply { get; set; }
        public string CustomerEmail { get; set; }

        [ForeignKey("CustomerReviewId")]
        public virtual CustomerReview CustomerReview { get; set; }
        public int CustomerReviewId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime UpdatedDate { get; set; }

    }
}