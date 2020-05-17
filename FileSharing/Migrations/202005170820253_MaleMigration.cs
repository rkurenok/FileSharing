namespace FileSharing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MaleMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "MaleId", "dbo.Males");
            DropIndex("dbo.Users", new[] { "MaleId" });
            AddColumn("dbo.Users", "Male", c => c.String());
            DropColumn("dbo.Users", "MaleId");
            DropTable("dbo.Males");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Males",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Users", "MaleId", c => c.Int(nullable: false));
            DropColumn("dbo.Users", "Male");
            CreateIndex("dbo.Users", "MaleId");
            AddForeignKey("dbo.Users", "MaleId", "dbo.Males", "Id", cascadeDelete: true);
        }
    }
}
