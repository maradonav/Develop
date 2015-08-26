using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foody.Models
{
    public class Role : BaseModel
    {
        [Key]
        public string RoleId { get; set; }
        [Display(Name = "Role ID")]
        public string RoleName { get; set; }

        [NotMapped]
        public List<Menu> Menus
        {
            get
            {
                using (DatabaseContext db = new DatabaseContext())
                {
                    var allMenus = db.MenuList.ToList();
                    var menuOfRoles = db.MenuRightList.Where(i => i.RoleId == RoleId).ToList();

                    for (int i = 0; i < allMenus.Count; i++)
                    {
                        bool selected = false;
                        Menu menu = allMenus[i];
                        if (menuOfRoles.FirstOrDefault(r => r.MenuId == menu.MenuId) != null)
                        {
                            selected = true;
                        }
                        menu.Selected = selected;
                    }

                    return allMenus;
                }
            }
        }

        public static List<Role> GetRoles()
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                return db.RoleList.ToList();
            }
        }
    }
}