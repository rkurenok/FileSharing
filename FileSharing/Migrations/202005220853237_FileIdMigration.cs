namespace FileSharing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileIdMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Users", "FileId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "FileId");
            DropColumn("dbo.Files", "Date");
        }
    }
}
