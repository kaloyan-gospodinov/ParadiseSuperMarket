﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EntityFramework.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ParadiseSupermarketChainEntities : DbContext
    {
        public ParadiseSupermarketChainEntities()
            : base("name=ParadiseSupermarketChainEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Dates> Dates { get; set; }
        public DbSet<Expenses> Expenses { get; set; }
        public DbSet<Measurements> Measurements { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Sales> Sales { get; set; }
        public DbSet<Supermarkets> Supermarkets { get; set; }
        public DbSet<sysdiagrams> sysdiagrams { get; set; }
        public DbSet<Vendors> Vendors { get; set; }
    }
}
