using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Web.Mvc;
using Foody.Models;

namespace Foody.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute
    {
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Ensure ASP.NET Simple Membership is initialized only once per app start
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        private class SimpleMembershipInitializer
        {
            public SimpleMembershipInitializer()
            {
                Database.SetInitializer<DatabaseContext>(null);

                try
                {
                    using (var context = new DatabaseContext())
                    {
                        if (!context.Database.Exists())
                        {
                            // Create the SimpleMembership database without Entity Framework migration schema
                            ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();

                            //Initial menu
                            string adminMenuId = Guid.NewGuid().ToString();
                            string youId = Guid.NewGuid().ToString();
                            context.MenuList.Add(new Menu { MenuName = "Home",HierarchyLevel= 0, MenuType = "P", ControllerName = "Home", ActionName = "Index", Ordinal = 0 });
                            context.MenuList.Add(new Menu { MenuName = "Admin", HierarchyLevel = 0, MenuId = adminMenuId, Ordinal = 1 });
                            context.MenuList.Add(new Menu { MenuName = "You", HierarchyLevel = 0, MenuId = youId, ControllerName = "Home", ActionName = "Index", Ordinal = 2 });
                            // context.MenuList.Add(new Menu { MenuName = "Quyền Hạn", ControllerName = "Account", ActionName = "ManageRole", ParentMenuId = accountMenuId, Ordinal = 1 });
                            context.MenuList.Add(new Menu { MenuName = "Booked", HierarchyLevel = 1, ControllerName = "Account", ActionName = "ChangPassword", ParentMenuId = youId, Ordinal = 0 });
                            context.MenuList.Add(new Menu { MenuName = "Unvalid", HierarchyLevel = 1, ControllerName = "Account", ActionName = "ChangPassword", ParentMenuId = youId, Ordinal = 1 });
                            context.MenuList.Add(new Menu { MenuName = "Menu Management", HierarchyLevel = 1, ControllerName = "Menu", ActionName = "Index", ParentMenuId = adminMenuId, Ordinal = 0 });
                            context.MenuList.Add(new Menu { MenuName = "Account Management", HierarchyLevel = 1, ControllerName = "Account", ActionName = "Index", ParentMenuId = adminMenuId, Ordinal = 1 });


                            //Save data changed
                            context.SaveChanges();
                        }
                    }

                    //WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }
        }
    }
}