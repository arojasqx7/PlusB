﻿using Domain.Entities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Domain.DAL
{
    public class PlusBContext : DbContext
    {
        public PlusBContext() : base("PlusBContext")
        {
        }
        public DbSet<Technology> Technologies {get; set;}
        public DbSet<Consultant> Consultants { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<TaskType> TaskTypes { get; set; }
        public DbSet<Severity> Severities { get; set; }
        public DbSet<Impact> Impacts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
