namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Item_Model : DbMigration
    {
        public override void Up()
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
                        UserId = c.Int(nullable: false),
                        CreatedDateTime = c.DateTime(nullable: false),
                        UpdatedDateTime = c.DateTime(nullable: false),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Items");
        }
    }
}
