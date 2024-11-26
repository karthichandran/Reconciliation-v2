using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data
{
    public class RolePermission
    {
        [Key]
        public int RolePermissionId { get; set; }
        public int RoleInfoId { get; set; }
        public int ScreenId { get; set; }
        public bool OptionId { get; set; }
        public bool CanView { get; set; }

        [NotMapped]
        public string SearchTerms { get; set; }
        public string BuildSearchTerms() => $"{RoleInfoId}".ToLower();


        [NotMapped]
        public string ScreenName { get; set; }
    }
}
