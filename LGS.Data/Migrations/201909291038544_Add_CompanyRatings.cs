namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_CompanyRatings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompanyRatings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rating = c.Single(nullable: false),
                        CompanyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.CompanyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompanyRatings", "CompanyId", "dbo.Companies");
            DropIndex("dbo.CompanyRatings", new[] { "CompanyId" });
            DropTable("dbo.CompanyRatings");
        }
    }
}
