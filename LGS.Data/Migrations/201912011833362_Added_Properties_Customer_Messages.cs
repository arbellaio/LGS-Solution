namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Properties_Customer_Messages : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerMessages", "CustomerFullName", c => c.String());
            AddColumn("dbo.CustomerMessages", "AddressOneUnit", c => c.String());
            AddColumn("dbo.CustomerMessages", "AddressTwoStreet", c => c.String());
            AddColumn("dbo.CustomerMessages", "AddressThreeLocality", c => c.String());
            AddColumn("dbo.CustomerMessages", "PostalCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerMessages", "PostalCode");
            DropColumn("dbo.CustomerMessages", "AddressThreeLocality");
            DropColumn("dbo.CustomerMessages", "AddressTwoStreet");
            DropColumn("dbo.CustomerMessages", "AddressOneUnit");
            DropColumn("dbo.CustomerMessages", "CustomerFullName");
        }
    }
}
