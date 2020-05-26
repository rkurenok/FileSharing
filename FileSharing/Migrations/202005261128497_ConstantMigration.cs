namespace FileSharing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConstantMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Constants", "Value", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Constants", "Value");
        }
    }
}
