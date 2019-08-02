namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Reset : DbMigration
    {
        public override void Up()
        {
//            CreateTable(
//                "dbo.AccountCredits",
//                c => new
//                    {
//                        Id = c.Int(nullable: false, identity: true),
//                        TotalCredits = c.Decimal(nullable: false, precision: 18, scale: 2),
//                        LastCreditTransaction = c.Decimal(nullable: false, precision: 18, scale: 2),
//                        LastCompanyCredited = c.Int(nullable: false),
//                        UserId = c.String(maxLength: 128),
//                        CreatedDate = c.DateTime(nullable: false),
//                        UpdatedDate = c.DateTime(nullable: false),
//                    })
//                .PrimaryKey(t => t.Id)
//                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
//                .Index(t => t.UserId);
//            
//            CreateTable(
//                "dbo.AspNetUsers",
//                c => new
//                    {
//                        Id = c.String(nullable: false, maxLength: 128),
//                        FullName = c.String(),
//                        Email = c.String(maxLength: 256),
//                        EmailConfirmed = c.Boolean(nullable: false),
//                        PasswordHash = c.String(),
//                        SecurityStamp = c.String(),
//                        PhoneNumber = c.String(),
//                        PhoneNumberConfirmed = c.Boolean(nullable: false),
//                        TwoFactorEnabled = c.Boolean(nullable: false),
//                        LockoutEndDateUtc = c.DateTime(),
//                        LockoutEnabled = c.Boolean(nullable: false),
//                        AccessFailedCount = c.Int(nullable: false),
//                        UserName = c.String(nullable: false, maxLength: 256),
//                    })
//                .PrimaryKey(t => t.Id)
//                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
//            
//            CreateTable(
//                "dbo.AspNetUserClaims",
//                c => new
//                    {
//                        Id = c.Int(nullable: false, identity: true),
//                        UserId = c.String(nullable: false, maxLength: 128),
//                        ClaimType = c.String(),
//                        ClaimValue = c.String(),
//                    })
//                .PrimaryKey(t => t.Id)
//                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
//                .Index(t => t.UserId);
//            
//            CreateTable(
//                "dbo.AspNetUserLogins",
//                c => new
//                    {
//                        LoginProvider = c.String(nullable: false, maxLength: 128),
//                        ProviderKey = c.String(nullable: false, maxLength: 128),
//                        UserId = c.String(nullable: false, maxLength: 128),
//                    })
//                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
//                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
//                .Index(t => t.UserId);
//            
//            CreateTable(
//                "dbo.AspNetUserRoles",
//                c => new
//                    {
//                        UserId = c.String(nullable: false, maxLength: 128),
//                        RoleId = c.String(nullable: false, maxLength: 128),
//                    })
//                .PrimaryKey(t => new { t.UserId, t.RoleId })
//                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
//                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
//                .Index(t => t.UserId)
//                .Index(t => t.RoleId);
//            
//            CreateTable(
//                "dbo.BusinessTypes",
//                c => new
//                    {
//                        Id = c.Int(nullable: false, identity: true),
//                        BusinessTypeName = c.String(),
//                    })
//                .PrimaryKey(t => t.Id);
//            
//            CreateTable(
//                "dbo.Clients",
//                c => new
//                    {
//                        Id = c.Int(nullable: false, identity: true),
//                        AppUserId = c.String(maxLength: 128),
//                        PhoneNumber = c.String(),
//                        FacebookPageLink = c.String(),
//                        CreatedDate = c.DateTime(nullable: false),
//                        UpdatedDate = c.DateTime(nullable: false),
//                    })
//                .PrimaryKey(t => t.Id)
//                .ForeignKey("dbo.AspNetUsers", t => t.AppUserId)
//                .Index(t => t.AppUserId);
//            
//            CreateTable(
//                "dbo.Companies",
//                c => new
//                    {
//                        Id = c.Int(nullable: false, identity: true),
//                        LogoPath = c.String(),
//                        CompanyName = c.String(),
//                        MainDescription = c.String(),
//                        LongDescription = c.String(),
//                        Ratings = c.Int(nullable: false),
//                        FacebookId = c.String(),
//                        FacebookPageLink = c.String(),
//                        GoogleId = c.String(),
//                        ClientId = c.Int(nullable: false),
//                        CreatedDate = c.DateTime(nullable: false),
//                        UpdatedDate = c.DateTime(nullable: false),
//                    })
//                .PrimaryKey(t => t.Id)
//                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
//                .Index(t => t.ClientId);
//            
//            CreateTable(
//                "dbo.CompanyCredits",
//                c => new
//                    {
//                        Id = c.Int(nullable: false, identity: true),
//                        Credits = c.Decimal(nullable: false, precision: 18, scale: 2),
//                        CompanyId = c.Int(nullable: false),
//                        UserId = c.String(maxLength: 128),
//                        CreatedDate = c.DateTime(nullable: false),
//                        UpdatedDate = c.DateTime(nullable: false),
//                    })
//                .PrimaryKey(t => t.Id)
//                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
//                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
//                .Index(t => t.CompanyId)
//                .Index(t => t.UserId);
//            
//            CreateTable(
//                "dbo.CompanyTypes",
//                c => new
//                    {
//                        Id = c.Int(nullable: false, identity: true),
//                        OtherType = c.String(),
//                        CompanyId = c.Int(nullable: false),
//                        BusinessTypeId = c.Int(nullable: false),
//                    })
//                .PrimaryKey(t => t.Id)
//                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
//                .Index(t => t.CompanyId);
//            
//            CreateTable(
//                "dbo.CreditInvoices",
//                c => new
//                    {
//                        Id = c.Int(nullable: false, identity: true),
//                        InvoiceNo = c.String(),
//                        TransactionId = c.String(),
//                        AccountId = c.String(),
//                        TransactionService = c.String(),
//                        TransactionAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
//                        CompanyId = c.Int(nullable: false),
//                        UserId = c.String(maxLength: 128),
//                        CreatedDate = c.DateTime(nullable: false),
//                        UpdatedDate = c.DateTime(nullable: false),
//                    })
//                .PrimaryKey(t => t.Id)
//                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
//                .Index(t => t.UserId);
//            
//            CreateTable(
//                "dbo.Customers",
//                c => new
//                    {
//                        Id = c.Int(nullable: false, identity: true),
//                        Email = c.String(),
//                        PhoneNumber = c.String(),
//                        CreatedDate = c.DateTime(nullable: false),
//                        UpdatedDate = c.DateTime(nullable: false),
//                    })
//                .PrimaryKey(t => t.Id);
//            
//            CreateTable(
//                "dbo.AspNetRoles",
//                c => new
//                    {
//                        Id = c.String(nullable: false, maxLength: 128),
//                        Name = c.String(nullable: false, maxLength: 256),
//                    })
//                .PrimaryKey(t => t.Id)
//                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
//            
//            CreateTable(
//                "dbo.SubAdmins",
//                c => new
//                    {
//                        Id = c.Int(nullable: false, identity: true),
//                        AppUserId = c.String(maxLength: 128),
//                        PhoneNumber = c.String(),
//                        CreatedDate = c.DateTime(nullable: false),
//                        UpdatedDate = c.DateTime(nullable: false),
//                    })
//                .PrimaryKey(t => t.Id)
//                .ForeignKey("dbo.AspNetUsers", t => t.AppUserId)
//                .Index(t => t.AppUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubAdmins", "AppUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.CreditInvoices", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CompanyTypes", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.CompanyCredits", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CompanyCredits", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Clients", "AppUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Companies", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.AccountCredits", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.SubAdmins", new[] { "AppUserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.CreditInvoices", new[] { "UserId" });
            DropIndex("dbo.CompanyTypes", new[] { "CompanyId" });
            DropIndex("dbo.CompanyCredits", new[] { "UserId" });
            DropIndex("dbo.CompanyCredits", new[] { "CompanyId" });
            DropIndex("dbo.Companies", new[] { "ClientId" });
            DropIndex("dbo.Clients", new[] { "AppUserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AccountCredits", new[] { "UserId" });
            DropTable("dbo.SubAdmins");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Customers");
            DropTable("dbo.CreditInvoices");
            DropTable("dbo.CompanyTypes");
            DropTable("dbo.CompanyCredits");
            DropTable("dbo.Companies");
            DropTable("dbo.Clients");
            DropTable("dbo.BusinessTypes");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AccountCredits");
        }
    }
}
