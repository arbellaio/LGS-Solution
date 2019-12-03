namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Properties_Reviews_Table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerReviews", "CustomerAddress", c => c.String());
            AddColumn("dbo.CustomerReviews", "ServiceRating", c => c.Single(nullable: false));
            AddColumn("dbo.CustomerReviews", "PriceRating", c => c.Single(nullable: false));
            AddColumn("dbo.CustomerReviews", "QualityRating", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerReviews", "QualityRating");
            DropColumn("dbo.CustomerReviews", "PriceRating");
            DropColumn("dbo.CustomerReviews", "ServiceRating");
            DropColumn("dbo.CustomerReviews", "CustomerAddress");
        }
    }
}
