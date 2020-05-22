namespace FileSharing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileIdRemoveMigration : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "FileId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "FileId", c => c.Int());
        }
    }
}
