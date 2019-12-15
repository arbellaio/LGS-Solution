using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LGS.Models.Leads
{
    public class FacebookLead
    {
        public int Id { get; set; }
        public string PageId { get; set; }
        public string PageAccessToken { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public List<FacebookLeadDetail> FacebookLeadDetails { get; set; }
    }
    
    public class FacebookLeadDetail
    {
        public int Id { get; set; }
        public string PageId { get; set; }
        public string PageAccessToken { get; set; }
        public string ColumnName { get; set; }
        public string ColumnValue { get; set; }
        [ForeignKey("FacebookLeadId")]
        public virtual FacebookLead FacebookLead { get; set; }
        public int FacebookLeadId { get; set; }

        public DateTime? CreatedDateTime { get; set; }

    }


}