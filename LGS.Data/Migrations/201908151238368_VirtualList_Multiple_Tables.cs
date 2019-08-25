namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VirtualList_Multiple_Tables : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountCredits", "Client_Id", c => c.Int());
            AddColumn("dbo.CompanyCredits", "Client_Id", c => c.Int());
            AddColumn("dbo.CreditInvoices", "Client_Id", c => c.Int());
            CreateIndex("dbo.AccountCredits", "Client_Id");
            CreateIndex("dbo.CompanyCredits", "Client_Id");
            CreateIndex("dbo.CreditInvoices", "Client_Id");
            AddForeignKey("dbo.AccountCredits", "Client_Id", "dbo.Clients", "Id");
            AddForeignKey("dbo.CompanyCredits", "Client_Id", "dbo.Clients", "Id");
            AddForeignKey("dbo.CreditInvoices", "Client_Id", "dbo.Clients", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CreditInvoices", "Client_Id", "dbo.Clients");
            DropForeignKey("dbo.CompanyCredits", "Client_Id", "dbo.Clients");
            DropForeignKey("dbo.AccountCredits", "Client_Id", "dbo.Clients");
            DropIndex("dbo.CreditInvoices", new[] { "Client_Id" });
            DropIndex("dbo.CompanyCredits", new[] { "Client_Id" });
            DropIndex("dbo.AccountCredits", new[] { "Client_Id" });
            DropColumn("dbo.CreditInvoices", "Client_Id");
            DropColumn("dbo.CompanyCredits", "Client_Id");
            DropColumn("dbo.AccountCredits", "Client_Id");
        }
    }
}
