using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data
{
    public class Groups
    {
        [Key]
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public bool IsActive { get; set; }
        public int GroupType { get; set; }
        [NotMapped]
        public string SearchTerms { get; set; }
        [NotMapped]
        public bool DoCopyGroup { get; set; }
        public string BuildSearchTerms() => $"{GroupName}".ToLower();
    }
}
