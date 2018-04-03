namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConsultantKPIUniqueIndex : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.KPI", "Name", c => c.String(maxLength: 450));
            CreateIndex("dbo.KPI", "Name", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.KPI", new[] { "Name" });
            AlterColumn("dbo.KPI", "Name", c => c.String());
        }
    }
}
