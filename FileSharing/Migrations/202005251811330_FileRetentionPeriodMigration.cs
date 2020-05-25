namespace FileSharing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileRetentionPeriodMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FileRetentionPeriods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Files", "FileRententionPeriodId", c => c.Int(nullable: false));
            AddColumn("dbo.Files", "FileRetentionPeriod_Id", c => c.Int());
            CreateIndex("dbo.Files", "FileRetentionPeriod_Id");
            AddForeignKey("dbo.Files", "FileRetentionPeriod_Id", "dbo.FileRetentionPeriods", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Files", "FileRetentionPeriod_Id", "dbo.FileRetentionPeriods");
            DropIndex("dbo.Files", new[] { "FileRetentionPeriod_Id" });
            DropColumn("dbo.Files", "FileRetentionPeriod_Id");
            DropColumn("dbo.Files", "FileRententionPeriodId");
            DropTable("dbo.FileRetentionPeriods");
        }
    }
}
