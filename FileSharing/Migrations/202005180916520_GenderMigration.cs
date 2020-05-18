namespace FileSharing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GenderMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Gender", c => c.String());
            DropColumn("dbo.Users", "Male");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Male", c => c.String());
            DropColumn("dbo.Users", "Gender");
        }
    }
}
