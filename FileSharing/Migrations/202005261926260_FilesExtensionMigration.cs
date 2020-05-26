namespace FileSharing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FilesExtensionMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "FilesExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Categories", "FilesExtension");
        }
    }
}
