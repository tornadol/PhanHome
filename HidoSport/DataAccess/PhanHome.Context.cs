﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PhanHomeEntities : DbContext
    {
        public PhanHomeEntities()
            : base("name=PhanHomeEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Cate> Cates { get; set; }
        public virtual DbSet<CateChild> CateChilds { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerBuy> CustomerBuys { get; set; }
        public virtual DbSet<ImageDetail> ImageDetails { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<New> News { get; set; }
        public virtual DbSet<Request> Requests { get; set; }
        public virtual DbSet<Admin_Login> Admin_Login { get; set; }
    }
}
