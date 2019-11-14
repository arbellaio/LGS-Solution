namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changes_In_ItemModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Items", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Items", "UserId", c => c.Int(nullable: false));
        }
    }
}
