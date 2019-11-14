namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changes_In_Credit_Account_Models : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountCredits", "AvailableCredits", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.AccountCredits", "TransactionId", c => c.String());
            AddColumn("dbo.AccountCredits", "InvoiceNo", c => c.String());
            DropColumn("dbo.AccountCredits", "LastCreditTransaction");
            DropColumn("dbo.AccountCredits", "LastCompanyCredited");
            DropColumn("dbo.CreditInvoices", "AccountId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CreditInvoices", "AccountId", c => c.String());
            AddColumn("dbo.AccountCredits", "LastCompanyCredited", c => c.Int(nullable: false));
            AddColumn("dbo.AccountCredits", "LastCreditTransaction", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.AccountCredits", "InvoiceNo");
            DropColumn("dbo.AccountCredits", "TransactionId");
            DropColumn("dbo.AccountCredits", "AvailableCredits");
        }
    }
}
