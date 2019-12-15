namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_FacebookLeads_FacebookLeadDetails : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FacebookLeads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PageId = c.String(),
                        PageAccessToken = c.String(),
                        CreatedDateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FacebookLeadDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PageId = c.String(),
                        PageAccessToken = c.String(),
                        ColumnName = c.String(),
                        ColumnValue = c.String(),
                        FacebookLeadId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FacebookLeads", t => t.FacebookLeadId, cascadeDelete: true)
                .Index(t => t.FacebookLeadId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FacebookLeadDetails", "FacebookLeadId", "dbo.FacebookLeads");
            DropIndex("dbo.FacebookLeadDetails", new[] { "FacebookLeadId" });
            DropTable("dbo.FacebookLeadDetails");
            DropTable("dbo.FacebookLeads");
        }
    }
}
