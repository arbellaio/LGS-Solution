namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccountCredit_Single : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AccountCredits", "ClientId", "dbo.Clients");
            DropIndex("dbo.AccountCredits", new[] { "ClientId" });
            AddColumn("dbo.Clients", "AccountCredit_Id", c => c.Int());
            CreateIndex("dbo.Clients", "AccountCredit_Id");
            AddForeignKey("dbo.Clients", "AccountCredit_Id", "dbo.AccountCredits", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Clients", "AccountCredit_Id", "dbo.AccountCredits");
            DropIndex("dbo.Clients", new[] { "AccountCredit_Id" });
            DropColumn("dbo.Clients", "AccountCredit_Id");
            CreateIndex("dbo.AccountCredits", "ClientId");
            AddForeignKey("dbo.AccountCredits", "ClientId", "dbo.Clients", "Id", cascadeDelete: true);
        }
    }
}
