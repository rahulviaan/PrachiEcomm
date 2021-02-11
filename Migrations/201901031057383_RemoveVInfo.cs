namespace PrachiIndia.Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveVInfo : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "VehicleType");
            DropColumn("dbo.AspNetUsers", "VehicleNo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "VehicleNo", c => c.String());
            AddColumn("dbo.AspNetUsers", "VehicleType", c => c.String());
        }
    }
}
