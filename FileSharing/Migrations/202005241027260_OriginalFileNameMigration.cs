namespace FileSharing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OriginalFileNameMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "OriginalName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Files", "OriginalName");
        }
    }
}
