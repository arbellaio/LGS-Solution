namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Company_Addresses_PostalCode_PhoneNumber_Email_Update : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "ShortDescription", c => c.String());
            AddColumn("dbo.Companies", "PhoneNumber", c => c.String());
            AddColumn("dbo.Companies", "AddressOneUnit", c => c.String());
            AddColumn("dbo.Companies", "AddressTwoStreet", c => c.String());
            AddColumn("dbo.Companies", "AddressThreeLocality", c => c.String());
            AddColumn("dbo.Companies", "PostalCode", c => c.String());
            AddColumn("dbo.Companies", "CompanyEmail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Companies", "CompanyEmail");
            DropColumn("dbo.Companies", "PostalCode");
            DropColumn("dbo.Companies", "AddressThreeLocality");
            DropColumn("dbo.Companies", "AddressTwoStreet");
            DropColumn("dbo.Companies", "AddressOneUnit");
            DropColumn("dbo.Companies", "PhoneNumber");
            DropColumn("dbo.Companies", "ShortDescription");
        }
    }
}
