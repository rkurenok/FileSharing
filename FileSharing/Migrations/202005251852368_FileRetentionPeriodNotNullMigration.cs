namespace FileSharing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileRetentionPeriodNotNullMigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Files", "FileRententionPeriodId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Files", "FileRententionPeriodId", c => c.Int());
        }
    }
}
