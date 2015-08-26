using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Foody.Models
{
    public class RoleEditModel
    {
        public string RoleId { get; set; }
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }

        public List<Menu> Menus { get; set; }
    }
}