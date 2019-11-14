namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changes_In_CreditInvoice_Model_Removed_Item_Model : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CreditInvoices", "Name", c => c.String());
            AddColumn("dbo.CreditInvoices", "Description", c => c.String());
            AddColumn("dbo.CreditInvoices", "SkuCode", c => c.String());
            AddColumn("dbo.CreditInvoices", "Quantity", c => c.String());
            AddColumn("dbo.CreditInvoices", "Price", c => c.String());
            AddColumn("dbo.CreditInvoices", "TotalPrice", c => c.String());
            AddColumn("dbo.CreditInvoices", "Tax", c => c.String());
            AddColumn("dbo.CreditInvoices", "Url", c => c.String());
            DropColumn("dbo.CreditInvoices", "CompanyId");
            DropTable("dbo.Items");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        currency = c.String(),
                        price = c.String(),
                        quantity = c.String(),
                        sku = c.String(),
                        description = c.String(),
                        tax = c.String(),
                        url = c.String(),
                        priceperitem = c.String(),
                        UserId = c.String(),
                        CreatedDateTime = c.DateTime(nullable: false),
                        UpdatedDateTime = c.DateTime(nullable: false),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            AddColumn("dbo.CreditInvoices", "CompanyId", c => c.Int(nullable: false));
            DropColumn("dbo.CreditInvoices", "Url");
            DropColumn("dbo.CreditInvoices", "Tax");
            DropColumn("dbo.CreditInvoices", "TotalPrice");
            DropColumn("dbo.CreditInvoices", "Price");
            DropColumn("dbo.CreditInvoices", "Quantity");
            DropColumn("dbo.CreditInvoices", "SkuCode");
            DropColumn("dbo.CreditInvoices", "Description");
            DropColumn("dbo.CreditInvoices", "Name");
        }
    }
}
