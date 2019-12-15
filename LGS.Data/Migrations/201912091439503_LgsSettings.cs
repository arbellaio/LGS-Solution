namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LgsSettings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LgsSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LeadsPerCredit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreditPerMoney = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LgsSettings");
        }
    }
}
