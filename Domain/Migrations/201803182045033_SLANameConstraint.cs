namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SLANameConstraint : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SLA", "Name", c => c.String(maxLength: 450));
            CreateIndex("dbo.SLA", "Name", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.SLA", new[] { "Name" });
            AlterColumn("dbo.SLA", "Name", c => c.String());
        }
    }
}
