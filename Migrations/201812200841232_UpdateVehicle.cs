namespace PrachiIndia.Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateVehicle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "VehicleType", c => c.String());
            AddColumn("dbo.AspNetUsers", "VehicleNo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "VehicleNo");
            DropColumn("dbo.AspNetUsers", "VehicleType");
        }
    }
}
