namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Customer_Review_Replies : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerReviewReplies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReviewReply = c.String(),
                        CustomerEmail = c.String(),
                        CustomerReviewId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerReviews", t => t.CustomerReviewId, cascadeDelete: true)
                .Index(t => t.CustomerReviewId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerReviewReplies", "CustomerReviewId", "dbo.CustomerReviews");
            DropIndex("dbo.CustomerReviewReplies", new[] { "CustomerReviewId" });
            DropTable("dbo.CustomerReviewReplies");
        }
    }
}
