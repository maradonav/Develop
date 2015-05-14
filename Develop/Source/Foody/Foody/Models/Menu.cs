using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Foody.Models
{
    public class Menu : BaseModel
    {
        [Key]
        public string MenuId { get; set; }
        [Display(Name = "Menu Name")]
        [Required]
        public string MenuName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string Parameters { get; set; }
        public int HierarchyLevel { get; set; }
        public int Ordinal { get; set; }
        public string MenuType { get; set; }
        public string ParentMenuId { get; set; }

        [NotMapped]
        public bool IsForEveryOne
        {
            get
            {
                return MenuType == "P";
            }
            set
            {
                MenuType = value ? "P" : string.Empty;
            }
        }

        [NotMapped]
        public bool IsForAuthenticatedUsers
        {
            get
            {
                return MenuType == "A";
            }
            set
            {
                MenuType = value ? "A" : string.Empty;
            }
        }

        [NotMapped]
        public bool IsForAuthorizedUsers
        {
            get
            {
                return MenuType == "";
            }
        }

        [NotMapped]
        public bool Selected { get; set; }

        public Menu()
        {
            MenuId = Guid.NewGuid().ToString();
            MenuName = string.Empty;
            ControllerName = string.Empty;
            ActionName = string.Empty;
            Parameters = string.Empty;
            ParentMenuId = string.Empty;
            HierarchyLevel = 0;
            Ordinal = 0;
            MenuType = string.Empty;
        }

        public static int GetMaxLevelMenu(string userName)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                var source = db.MenuList;
                List<Menu> result = new List<Menu>();
                foreach (Menu menu in source)
                {
                    if (IsVisibleFor(menu.MenuId, userName))
                    {
                        result.Add(menu);
                    }
                }
                int maxLevel = result.Max(i => i.HierarchyLevel);
                return maxLevel;
            }
        }
        public string GetMenuUrl()
        {
            string url = string.Empty;

            if (string.IsNullOrEmpty(ControllerName) || string.IsNullOrEmpty(ActionName))
            {
                url = "#";
            }
            else
            {
                url = string.Format("/{0}/{1}/{2}", ControllerName, ActionName, Parameters);
            }

            return url;
        }

        public static List<Menu> GetMenus(string parentMenuID, int hierarchyLevel, string userName)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                var source = db.MenuList.Where(i => i.HierarchyLevel == hierarchyLevel && i.ParentMenuId == parentMenuID);
                List<Menu> result = new List<Menu>();
                foreach (Menu menu in source)
                {
                    if (IsVisibleFor(menu.MenuId, userName))
                    {
                        result.Add(menu);
                    }
                }
                return result.OrderBy(i => i.Ordinal).ToList();
            }
        }

        public static bool IsVisibleFor(string menuId, string userName)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                Menu menu = db.MenuList.First(i => i.MenuId == menuId);
                if (menu != null)
                {
                    /*If menu type is "P" (for all users) => return true
                     */
                    if (menu.MenuType == "P")
                    {
                        return true;
                    }
                    else if (!string.IsNullOrEmpty(userName))
                    {
                        /*If menu type is "A" (for all authenticated users) => return true
                         */
                        if (menu.MenuType == "A")
                        {
                            if (userName == "admin" || userName == "maradonav")
                            {
                                return true;                                
                            }
                        }
                        else
                        {
                            /*If user has role which is authorized for the menu => return true
                             */
                            //int menuRightsCount = db.MenuRightList.Where(i => i.MenuId == menuId).Join(
                            //    db.UserInRoleList.Where(i => i.UserName == userName),
                            //    menuRight => menuRight.RoleId,
                            //    userInRole => userInRole.RoleId,
                            //    (menuRight, userInRole) => menuRight).Count();
                            //if (menuRightsCount > 0)
                            //{
                            //    return true;
                            //}
                            //else
                            //{
                            //    /*If the menu has child, check if it's child is visible
                            //     */
                            //    List<string> childMenus = db.MenuList.Where(i => i.ParentMenuId == menuId).Select(i => i.MenuId).ToList();
                            //    if (childMenus.Count > 0)
                            //    {
                            //        foreach (string childMenuId in childMenus)
                            //        {
                            //            bool isVisible = IsVisibleFor(childMenuId, userName);
                            //            if (isVisible)
                            //            {
                            //                return true;
                            //            }
                            //        }
                            //    }
                            //}
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}