using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Foody.Models
{
    public class BaseModel
    {
            public DateTime CreatedDateTime { get; set; }
            [Display(Name = "Cập nhật lần cuối")]
            public DateTime LastUpdatedDateTime { get; set; }
            public string CreatedBy { get; set; }
            [Display(Name = "Cập nhật bởi")]
            public string LastUpdatedBy { get; set; }

            public BaseModel()
            {
                CreatedDateTime = DateTime.Now;
                LastUpdatedDateTime = DateTime.Now;
                CreatedBy = "System";
                LastUpdatedBy = "System";
            }

            public void SetUpdatedInfo(string userName)
            {
                LastUpdatedDateTime = DateTime.Now;
                LastUpdatedBy = string.IsNullOrEmpty(userName) ? "System" : userName;
            }
    }
}