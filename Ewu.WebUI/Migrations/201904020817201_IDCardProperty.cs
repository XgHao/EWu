namespace Ewu.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IDCardProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "IDCardImageData", c => c.Binary());
            AddColumn("dbo.AspNetUsers", "IDCardImageMimeType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "IDCardImageMimeType");
            DropColumn("dbo.AspNetUsers", "IDCardImageData");
        }
    }
}
