namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changes_In_CreditInvoice_Currency : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CreditInvoices", "Currency", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CreditInvoices", "Currency");
        }
    }
}
