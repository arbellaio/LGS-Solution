namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Company_Invoice_Items_CompanyCredit : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CompanyCredits", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CompanyCredits", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.CompanyCredits", "Client_Id", "dbo.Clients");
            DropIndex("dbo.CompanyCredits", new[] { "CompanyId" });
            DropIndex("dbo.CompanyCredits", new[] { "UserId" });
            DropIndex("dbo.CompanyCredits", new[] { "Client_Id" });
            AddColumn("dbo.Companies", "CompanyCreditsUsed", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Companies", "RemainingCredits", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Companies", "CreditUseDateTime", c => c.DateTime(nullable: false));
            DropTable("dbo.CompanyCredits");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CompanyCredits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreditsUsed = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RemainingCredits = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CompanyId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        Client_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.Companies", "CreditUseDateTime");
            DropColumn("dbo.Companies", "RemainingCredits");
            DropColumn("dbo.Companies", "CompanyCreditsUsed");
            CreateIndex("dbo.CompanyCredits", "Client_Id");
            CreateIndex("dbo.CompanyCredits", "UserId");
            CreateIndex("dbo.CompanyCredits", "CompanyId");
            AddForeignKey("dbo.CompanyCredits", "Client_Id", "dbo.Clients", "Id");
            AddForeignKey("dbo.CompanyCredits", "CompanyId", "dbo.Companies", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CompanyCredits", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
