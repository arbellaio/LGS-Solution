using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using LGS.Models.Companies;

namespace LGS.Models.Leads
{
    public class GoogleLead
    {
        public int Id { get; set; }
        public string LeadId { get; set; }
        public int CampaignId { get; set; }
        public string GclId { get; set; }
        public string ApiVersion { get; set; }
        public int FormId { get; set; }
        public string GoogleKey { get; set; }
        public bool IsTest { get; set; }
        public virtual List<GoogleLeadDetail> GoogleLeadDetails { get; set; }

    }

    public class GoogleLeadDetail
    {
        public int Id { get; set; }
        public string LeadId { get; set; }
        public int CampaignId { get; set; }
        public string GclId { get; set; }
        public int FormId { get; set; }
        public string GoogleKey { get; set; }
        public string ColumnName { get; set; }
        public string ColumnValue { get; set; }

        [ForeignKey("GoogleLeadId")]
        public virtual GoogleLead GoogleLead { get; set; }
        public int GoogleLeadId { get; set; }
    }

    public class CompanyGoogleKey {
        public int Id { get; set; }
        public string GoogleAdKey { get; set; }
        public int CompanyId { get; set; }
        
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

    }
}
