using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LGS.Models.Users
{
    public class SubAdmin
    {
        public int Id { get; set; }

        [ForeignKey("AppUserId")]
        public virtual ApplicationUser User { get; set; }
        public string AppUserId { get; set; }
        public string ProfilePhoto { get; set; }
        public bool IsBlocked { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime UpdatedDate { get; set; }

    }
}