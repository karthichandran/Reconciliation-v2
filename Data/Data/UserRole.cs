
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data
{
	public class UserRole
    {
        [Key]
        public int UserRoleId { get; set; }
        public int UserInfoId { get; set; }
        public int RoleId { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public string UpdatedBy { get; set; }
        [NotMapped]
        public string SearchTerms { get; set; }
        [NotMapped]
        public string Name { get; set; }
        [NotMapped]
        public bool IsSelected { get; set; }
        public string BuildSearchTerms() => $"{UserInfoId} {RoleId}".ToLower();
    }
}
