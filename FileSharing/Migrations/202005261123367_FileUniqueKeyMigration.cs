namespace FileSharing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileUniqueKeyMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FileUniqueKeys",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UniqueKey = c.String(),
                        FileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Files", "FileUniqueKeyId", c => c.Int());
            CreateIndex("dbo.Files", "FileUniqueKeyId");
            AddForeignKey("dbo.Files", "FileUniqueKeyId", "dbo.FileUniqueKeys", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Files", "FileUniqueKeyId", "dbo.FileUniqueKeys");
            DropIndex("dbo.Files", new[] { "FileUniqueKeyId" });
            DropColumn("dbo.Files", "FileUniqueKeyId");
            DropTable("dbo.FileUniqueKeys");
        }
    }
}
