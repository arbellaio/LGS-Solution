namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_New_Columns_Delivery_Notification_LeadLimit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "DeliveryInterval", c => c.Int());
            AddColumn("dbo.Companies", "NotificationMode", c => c.Int());
            AddColumn("dbo.Companies", "LeadLimit", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Companies", "LeadLimit");
            DropColumn("dbo.Companies", "NotificationMode");
            DropColumn("dbo.Companies", "DeliveryInterval");
        }
    }
}
