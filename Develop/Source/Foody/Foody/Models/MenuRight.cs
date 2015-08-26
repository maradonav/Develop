﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foody.Models
{
    public class MenuRight : BaseModel
    {
        [Key, Column(Order = 0)]
        public string MenuId { get; set; }
        [Key, Column(Order = 1)]
        public string RoleId { get; set; }
    }
}