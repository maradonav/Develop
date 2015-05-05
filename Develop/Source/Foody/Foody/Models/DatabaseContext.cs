using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Foody.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
            : base(CodeMaster.DefaultConnectionStringName)
        {

        }
        //public DbSet<Account> AccountList { get; set; }
        //public DbSet<Menu> MenuList { get; set; }
        //public DbSet<Table> TableList { get; set; }
        //public DbSet<OrderTable> OrderTableList { get; set; }
        //public DbSet<OrderFood> OrderFoodList { get; set; }


        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Account>().ToTable("fptAccount");
        //    modelBuilder.Entity<Menu>().ToTable("fptMenu");
        //    modelBuilder.Entity<Table>().ToTable("fptTable");
        //    modelBuilder.Entity<OrderTable>().ToTable("fptOrderTable");
        //    modelBuilder.Entity<OrderFood>().ToTable("fptOrderFood");

        //    base.OnModelCreating(modelBuilder);
        //}
    }
}