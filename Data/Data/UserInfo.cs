using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data
{
    public class UserInfo
    {
        [Key]
        public int UserInfoId { get; set; }
        public string UserName { get; set; }
        public string loginName { get; set; }
        public string UserPassword { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public string UpdatedBy { get; set; }
        [NotMapped]
        public string SearchTerms { get; set; }
        public string BuildSearchTerms() => $"{UserName} {loginName}".ToLower();
    }
}

