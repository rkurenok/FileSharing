namespace FileSharing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Males",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Users", "Email", c => c.String());
            AddColumn("dbo.Users", "Age", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "MaleId", c => c.Int(nullable: true));
            CreateIndex("dbo.Users", "MaleId");
            AddForeignKey("dbo.Users", "MaleId", "dbo.Males", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "MaleId", "dbo.Males");
            DropIndex("dbo.Users", new[] { "MaleId" });
            DropColumn("dbo.Users", "MaleId");
            DropColumn("dbo.Users", "Age");
            DropColumn("dbo.Users", "Email");
            DropTable("dbo.Males");
        }
    }
}
