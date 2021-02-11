namespace PrachiIndia.Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "ImageUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "ImageUrl", c => c.String());
        }
    }
}
