namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Company_Inventory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompanyInventories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemId = c.Int(nullable: false),
                        ItemName = c.String(),
                        ItemSkuCode = c.String(),
                        ItemsUsed = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RemainingItems = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalItemBought = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UserId = c.String(maxLength: 128),
                        CompanyId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.CompanyId);
            
            DropColumn("dbo.Companies", "CompanyCreditsUsed");
            DropColumn("dbo.Companies", "RemainingCredits");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Companies", "RemainingCredits", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Companies", "CompanyCreditsUsed", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropForeignKey("dbo.CompanyInventories", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CompanyInventories", "CompanyId", "dbo.Companies");
            DropIndex("dbo.CompanyInventories", new[] { "CompanyId" });
            DropIndex("dbo.CompanyInventories", new[] { "UserId" });
            DropTable("dbo.CompanyInventories");
        }
    }
}
