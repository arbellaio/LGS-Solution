namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_CreatedDate_UpdatedDate_In_CustomerMessage_CustomerReview : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerMessages", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.CustomerMessages", "UpdatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.CustomerReviews", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.CustomerReviews", "UpdatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Customers", "CompanyId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "CompanyId");
            DropColumn("dbo.CustomerReviews", "UpdatedDate");
            DropColumn("dbo.CustomerReviews", "CreatedDate");
            DropColumn("dbo.CustomerMessages", "UpdatedDate");
            DropColumn("dbo.CustomerMessages", "CreatedDate");
        }
    }
}
