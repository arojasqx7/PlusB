namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEducationToConsultant : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Consultant", "Education", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Consultant", "Education");
        }
    }
}
