using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Foody.Models
{
    public class BaseModel
    {
        [Display(Name = "Created Date Time")]
        public DateTime CreatedDateTime { get; set; }
        [Display(Name = "Last Updated Date Time")]
        public DateTime LastUpdatedDateTime { get; set; }
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }
        [Display(Name = "Last Updated By")]
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