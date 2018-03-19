namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFK_SLACustomer : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CustomerSLA", "Customer_Id", "dbo.Customer");
            DropForeignKey("dbo.CustomerSLA", "SLA_ID", "dbo.SLA");
            DropIndex("dbo.CustomerSLA", new[] { "Customer_Id" });
            DropIndex("dbo.CustomerSLA", new[] { "SLA_ID" });
            DropColumn("dbo.CustomerSLA", "IdCustomer");
            DropColumn("dbo.CustomerSLA", "IdSLA");
            RenameColumn(table: "dbo.CustomerSLA", name: "Customer_Id", newName: "IdCustomer");
            RenameColumn(table: "dbo.CustomerSLA", name: "SLA_ID", newName: "IdSLA");
            AlterColumn("dbo.CustomerSLA", "IdCustomer", c => c.Int(nullable: false));
            AlterColumn("dbo.CustomerSLA", "IdSLA", c => c.Int(nullable: false));
            CreateIndex("dbo.CustomerSLA", "IdCustomer");
            CreateIndex("dbo.CustomerSLA", "IdSLA");
            AddForeignKey("dbo.CustomerSLA", "IdCustomer", "dbo.Customer", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CustomerSLA", "IdSLA", "dbo.SLA", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerSLA", "IdSLA", "dbo.SLA");
            DropForeignKey("dbo.CustomerSLA", "IdCustomer", "dbo.Customer");
            DropIndex("dbo.CustomerSLA", new[] { "IdSLA" });
            DropIndex("dbo.CustomerSLA", new[] { "IdCustomer" });
            AlterColumn("dbo.CustomerSLA", "IdSLA", c => c.Int());
            AlterColumn("dbo.CustomerSLA", "IdCustomer", c => c.Int());
            RenameColumn(table: "dbo.CustomerSLA", name: "IdSLA", newName: "SLA_ID");
            RenameColumn(table: "dbo.CustomerSLA", name: "IdCustomer", newName: "Customer_Id");
            AddColumn("dbo.CustomerSLA", "IdSLA", c => c.Int(nullable: false));
            AddColumn("dbo.CustomerSLA", "IdCustomer", c => c.Int(nullable: false));
            CreateIndex("dbo.CustomerSLA", "SLA_ID");
            CreateIndex("dbo.CustomerSLA", "Customer_Id");
            AddForeignKey("dbo.CustomerSLA", "SLA_ID", "dbo.SLA", "ID");
            AddForeignKey("dbo.CustomerSLA", "Customer_Id", "dbo.Customer", "Id");
        }
    }
}
