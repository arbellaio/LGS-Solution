namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Client_Id_ForeignKey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AccountCredits", "Client_Id", "dbo.Clients");
            DropForeignKey("dbo.CreditInvoices", "Client_Id", "dbo.Clients");
            DropIndex("dbo.AccountCredits", new[] { "Client_Id" });
            DropIndex("dbo.CreditInvoices", new[] { "Client_Id" });
            RenameColumn(table: "dbo.AccountCredits", name: "Client_Id", newName: "ClientId");
            RenameColumn(table: "dbo.CreditInvoices", name: "Client_Id", newName: "ClientId");
            AlterColumn("dbo.AccountCredits", "ClientId", c => c.Int(nullable: false));
            AlterColumn("dbo.CreditInvoices", "ClientId", c => c.Int(nullable: false));
            CreateIndex("dbo.AccountCredits", "ClientId");
            CreateIndex("dbo.CreditInvoices", "ClientId");
            AddForeignKey("dbo.AccountCredits", "ClientId", "dbo.Clients", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CreditInvoices", "ClientId", "dbo.Clients", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CreditInvoices", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.AccountCredits", "ClientId", "dbo.Clients");
            DropIndex("dbo.CreditInvoices", new[] { "ClientId" });
            DropIndex("dbo.AccountCredits", new[] { "ClientId" });
            AlterColumn("dbo.CreditInvoices", "ClientId", c => c.Int());
            AlterColumn("dbo.AccountCredits", "ClientId", c => c.Int());
            RenameColumn(table: "dbo.CreditInvoices", name: "ClientId", newName: "Client_Id");
            RenameColumn(table: "dbo.AccountCredits", name: "ClientId", newName: "Client_Id");
            CreateIndex("dbo.CreditInvoices", "Client_Id");
            CreateIndex("dbo.AccountCredits", "Client_Id");
            AddForeignKey("dbo.CreditInvoices", "Client_Id", "dbo.Clients", "Id");
            AddForeignKey("dbo.AccountCredits", "Client_Id", "dbo.Clients", "Id");
        }
    }
}
