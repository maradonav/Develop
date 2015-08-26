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
        public DbSet<Account> AccountList { get; set; }
        public DbSet<Menu> MenuList { get; set; }
        public DbSet<Role> RoleList { get; set; }
        public DbSet<UserInRole> UserInRoleList { get; set; }
        public DbSet<MenuRight> MenuRightList { get; set; }
        //public DbSet<Table> TableList { get; set; }
        //public DbSet<OrderTable> OrderTableList { get; set; }
        //public DbSet<OrderFood> OrderFoodList { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().ToTable("f_Account");
            modelBuilder.Entity<Menu>().ToTable("f_Menu");
            modelBuilder.Entity<Role>().ToTable("f_Role");
            modelBuilder.Entity<UserInRole>().ToTable("f_UserInRole");
            modelBuilder.Entity<MenuRight>().ToTable("f_MenuRight");
            //modelBuilder.Entity<Table>().ToTable("fptTable");
            //modelBuilder.Entity<OrderTable>().ToTable("fptOrderTable");
            //modelBuilder.Entity<OrderFood>().ToTable("fptOrderFood");

            base.OnModelCreating(modelBuilder);
        }
    }
}