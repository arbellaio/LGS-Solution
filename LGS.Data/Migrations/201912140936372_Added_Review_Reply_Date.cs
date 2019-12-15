namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Review_Reply_Date : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerReviews", "ReviewReplyDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerReviews", "ReviewReplyDate");
        }
    }
}
