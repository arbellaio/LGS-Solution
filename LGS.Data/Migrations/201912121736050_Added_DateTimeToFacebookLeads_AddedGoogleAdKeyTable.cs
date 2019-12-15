namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_DateTimeToFacebookLeads_AddedGoogleAdKeyTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompanyGoogleKeys",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GoogleAdKey = c.String(),
                        CompanyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.CompanyId);
            
            AddColumn("dbo.FacebookLeadDetails", "CreatedDateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompanyGoogleKeys", "CompanyId", "dbo.Companies");
            DropIndex("dbo.CompanyGoogleKeys", new[] { "CompanyId" });
            DropColumn("dbo.FacebookLeadDetails", "CreatedDateTime");
            DropTable("dbo.CompanyGoogleKeys");
        }
    }
}
