namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Google_Lead_Google_Lead_Details : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GoogleLeadDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LeadId = c.String(),
                        CampaignId = c.Int(nullable: false),
                        GclId = c.String(),
                        FormId = c.Int(nullable: false),
                        GoogleKey = c.String(),
                        ColumnName = c.String(),
                        ColumnValue = c.String(),
                        GoogleLeadId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GoogleLeads", t => t.GoogleLeadId, cascadeDelete: true)
                .Index(t => t.GoogleLeadId);
            
            CreateTable(
                "dbo.GoogleLeads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LeadId = c.String(),
                        CampaignId = c.Int(nullable: false),
                        GclId = c.String(),
                        ApiVersion = c.String(),
                        FormId = c.Int(nullable: false),
                        GoogleKey = c.String(),
                        IsTest = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GoogleLeadDetails", "GoogleLeadId", "dbo.GoogleLeads");
            DropIndex("dbo.GoogleLeadDetails", new[] { "GoogleLeadId" });
            DropTable("dbo.GoogleLeads");
            DropTable("dbo.GoogleLeadDetails");
        }
    }
}
