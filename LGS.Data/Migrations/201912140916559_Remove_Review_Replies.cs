namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Remove_Review_Replies : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerReviews", "ReviewReply", c => c.String());
            DropTable("dbo.CustomerReviewReplies");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CustomerReviewReplies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReviewReply = c.String(),
                        CustomerEmail = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.CustomerReviews", "ReviewReply");
        }
    }
}
