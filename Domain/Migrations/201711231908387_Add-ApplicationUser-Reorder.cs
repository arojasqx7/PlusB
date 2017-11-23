namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddApplicationUserReorder : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Consultant", "Password");
            DropColumn("dbo.Consultant", "Role");
            DropTable("dbo.LoginUser");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.LoginUser",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Email = c.String(),
                        Password = c.String(),
                        Role = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Consultant", "Role", c => c.String());
            AddColumn("dbo.Consultant", "Password", c => c.String());
        }
    }
}
