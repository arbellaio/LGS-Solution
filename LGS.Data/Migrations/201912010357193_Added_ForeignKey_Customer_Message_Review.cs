namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_ForeignKey_Customer_Message_Review : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.CustomerMessages", "CompanyId");
            CreateIndex("dbo.CustomerReviews", "CompanyId");
            AddForeignKey("dbo.CustomerMessages", "CompanyId", "dbo.Companies", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CustomerReviews", "CompanyId", "dbo.Companies", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerReviews", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.CustomerMessages", "CompanyId", "dbo.Companies");
            DropIndex("dbo.CustomerReviews", new[] { "CompanyId" });
            DropIndex("dbo.CustomerMessages", new[] { "CompanyId" });
        }
    }
}
