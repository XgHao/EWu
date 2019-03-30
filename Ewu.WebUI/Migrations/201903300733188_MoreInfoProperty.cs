namespace Ewu.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoreInfoProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "HeadPortrait", c => c.String());
            AddColumn("dbo.AspNetUsers", "Gender", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "Signature", c => c.String());
            AddColumn("dbo.AspNetUsers", "RealName", c => c.String());
            AddColumn("dbo.AspNetUsers", "BirthDay", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "Age", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "IDCardNO", c => c.String());
            AddColumn("dbo.AspNetUsers", "NativePlace", c => c.String());
            AddColumn("dbo.AspNetUsers", "OICQ", c => c.String());
            AddColumn("dbo.AspNetUsers", "WeChat", c => c.String());
            AddColumn("dbo.AspNetUsers", "Job", c => c.String());
            AddColumn("dbo.AspNetUsers", "RegisterTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "IsShowInfo", c => c.String());
            AddColumn("dbo.AspNetUsers", "CreditWorthiness", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.AspNetUsers", "TempDeductionValue", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.AspNetUsers", "MultipleDeduct", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.AspNetUsers", "PenaltyTime", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "Notice", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "Favorite", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Favorite");
            DropColumn("dbo.AspNetUsers", "Notice");
            DropColumn("dbo.AspNetUsers", "PenaltyTime");
            DropColumn("dbo.AspNetUsers", "MultipleDeduct");
            DropColumn("dbo.AspNetUsers", "TempDeductionValue");
            DropColumn("dbo.AspNetUsers", "CreditWorthiness");
            DropColumn("dbo.AspNetUsers", "IsShowInfo");
            DropColumn("dbo.AspNetUsers", "RegisterTime");
            DropColumn("dbo.AspNetUsers", "Job");
            DropColumn("dbo.AspNetUsers", "WeChat");
            DropColumn("dbo.AspNetUsers", "OICQ");
            DropColumn("dbo.AspNetUsers", "NativePlace");
            DropColumn("dbo.AspNetUsers", "IDCardNO");
            DropColumn("dbo.AspNetUsers", "Age");
            DropColumn("dbo.AspNetUsers", "BirthDay");
            DropColumn("dbo.AspNetUsers", "RealName");
            DropColumn("dbo.AspNetUsers", "Signature");
            DropColumn("dbo.AspNetUsers", "Gender");
            DropColumn("dbo.AspNetUsers", "HeadPortrait");
        }
    }
}
