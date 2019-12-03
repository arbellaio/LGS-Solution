namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Customer_Message_Review_Properties_Update_Customer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        CustomerEmail = c.String(),
                        CustomerPhoneNumber = c.String(),
                        CompanyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomerReviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Review = c.String(),
                        CustomerEmail = c.String(),
                        CustomerName = c.String(),
                        CustomerPhoneNumber = c.String(),
                        CompanyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Customers", "FullName", c => c.String());
            AddColumn("dbo.Customers", "AddressOneUnit", c => c.String());
            AddColumn("dbo.Customers", "AddressTwoStreet", c => c.String());
            AddColumn("dbo.Customers", "AddressThreeLocality", c => c.String());
            AddColumn("dbo.Customers", "PostalCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "PostalCode");
            DropColumn("dbo.Customers", "AddressThreeLocality");
            DropColumn("dbo.Customers", "AddressTwoStreet");
            DropColumn("dbo.Customers", "AddressOneUnit");
            DropColumn("dbo.Customers", "FullName");
            DropTable("dbo.CustomerReviews");
            DropTable("dbo.CustomerMessages");
        }
    }
}
