﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AllWebApi.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CombineApiEntities : DbContext
    {
        public CombineApiEntities()
            : base("name=CombineApiEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<empData> empDatas { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Practice_Student> Practice_Student { get; set; }
        public virtual DbSet<purchase> purchases { get; set; }
        public virtual DbSet<purchase_product> purchase_product { get; set; }
        public virtual DbSet<registration> registrations { get; set; }
        public virtual DbSet<AllFile> AllFiles { get; set; }
        public virtual DbSet<DocPat> DocPats { get; set; }
        public virtual DbSet<Image> Images { get; set; }
    }
}
