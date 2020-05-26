namespace FileSharing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoryMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Files", "CategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.Files", "CategoryId");
            AddForeignKey("dbo.Files", "CategoryId", "dbo.Categories", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Files", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Files", new[] { "CategoryId" });
            DropColumn("dbo.Files", "CategoryId");
            DropTable("dbo.Categories");
        }
    }
}
