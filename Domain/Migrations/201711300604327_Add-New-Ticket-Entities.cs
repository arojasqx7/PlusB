namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewTicketEntities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Impact",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImpactName = c.String(),
                        ImpactNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Severity",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SeverityName = c.String(),
                        SeverityNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TaskType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TaskName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TaskType");
            DropTable("dbo.Severity");
            DropTable("dbo.Impact");
        }
    }
}
