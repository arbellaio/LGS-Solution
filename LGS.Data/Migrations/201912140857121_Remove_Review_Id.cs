namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Remove_Review_Id : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CustomerReviewReplies", "CustomerReviewId", "dbo.CustomerReviews");
            DropIndex("dbo.CustomerReviewReplies", new[] { "CustomerReviewId" });
            DropColumn("dbo.CustomerReviewReplies", "CustomerReviewId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CustomerReviewReplies", "CustomerReviewId", c => c.Int(nullable: false));
            CreateIndex("dbo.CustomerReviewReplies", "CustomerReviewId");
            AddForeignKey("dbo.CustomerReviewReplies", "CustomerReviewId", "dbo.CustomerReviews", "Id", cascadeDelete: true);
        }
    }
}
