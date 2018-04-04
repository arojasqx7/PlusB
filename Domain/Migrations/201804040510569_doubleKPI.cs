namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class doubleKPI : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.KPIEvaluation", "Score", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.KPIEvaluation", "Score", c => c.Single(nullable: false));
        }
    }
}
