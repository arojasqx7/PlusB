namespace Domain.Migrations
{
    using Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Domain.DAL.PlusBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Domain.DAL.PlusBContext context)
        {
            //  This method will be called after migrating to the latest version.

            context.Customers.AddOrUpdate(
             p => p.Id,
             new Customer
             {
                 Id = 1,
                 CompanyName = "PlusB Consulting",
                 CompanyId = "ADM",
                 CompanyDescription = "Consulting Provider",
                 ManagerName = "Francisco Alvarado",
                 Country = "Costa Rica",
                 Address = "Escazu",
                 PhoneNumber = 22451215,
                 EmailAddress = "info@plusb.com",
                 SupportType = null,
                 InitialDate = Convert.ToDateTime("01/01/2000")
             }
           );

            context.Customers.AddOrUpdate(
              p => p.Id,
              new Customer
              {
                  Id = 2,
                  CompanyName = "Sapiens Software",
                  CompanyId = "CUST1",
                  CompanyDescription = "Software Development",
                  ManagerName = "Alcidez Lopez",
                  Country = "Costa Rica",
                  Address = "San Jose",
                  PhoneNumber = 22560012,
                  EmailAddress = "info@sapiensdev.com",
                  SupportType = null,
                  InitialDate = Convert.ToDateTime("01/01/2000")
              }
            );

            context.Consultants.AddOrUpdate(
                p => p.ID,
                new Consultant
                {
                    ID = 1,
                    FirstName = "Unassigned",
                    LastName = "Unassigned",
                    DateOfBirth = Convert.ToDateTime("01/01/2000"),
                    IdNumber = "1",
                    Gender = "Male",
                    Email = "info@plusb.com",
                    Pais = "Costa Rica",
                    Address = "Escazu",
                    PhoneNumber = 22841036,
                    JobTitle = "Admin",
                    HireDate = Convert.ToDateTime("01/01/2000")
                }
                );

        }
    }
}
