namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccountCredit_Changes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Clients", "AccountCredit_Id", "dbo.AccountCredits");
            DropIndex("dbo.Clients", new[] { "AccountCredit_Id" });
            RenameColumn(table: "dbo.Clients", name: "AccountCredit_Id", newName: "AccountCreditId");
            AlterColumn("dbo.Clients", "AccountCreditId", c => c.Int(nullable: false));
            CreateIndex("dbo.Clients", "AccountCreditId");
            AddForeignKey("dbo.Clients", "AccountCreditId", "dbo.AccountCredits", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Clients", "AccountCreditId", "dbo.AccountCredits");
            DropIndex("dbo.Clients", new[] { "AccountCreditId" });
            AlterColumn("dbo.Clients", "AccountCreditId", c => c.Int());
            RenameColumn(table: "dbo.Clients", name: "AccountCreditId", newName: "AccountCredit_Id");
            CreateIndex("dbo.Clients", "AccountCredit_Id");
            AddForeignKey("dbo.Clients", "AccountCredit_Id", "dbo.AccountCredits", "Id");
        }
    }
}
