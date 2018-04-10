namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SurveyFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Survey", "IsAnswered", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Survey", "IsAnswered");
        }
    }
}
