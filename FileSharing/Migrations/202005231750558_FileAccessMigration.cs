namespace FileSharing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileAccessMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FileAccesses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Files", "AccessId", c => c.Int(nullable: false));
            CreateIndex("dbo.Files", "AccessId");
            AddForeignKey("dbo.Files", "AccessId", "dbo.FileAccesses", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Files", "AccessId", "dbo.FileAccesses");
            DropIndex("dbo.Files", new[] { "AccessId" });
            DropColumn("dbo.Files", "AccessId");
            DropTable("dbo.FileAccesses");
        }
    }
}
