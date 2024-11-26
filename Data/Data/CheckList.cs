using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data
{
	public class CheckList
    {
        [Key]
        public int CheckListId { get; set; }
        public Guid CheckListGuid { get; set; }
        public string CheckListName { get; set; }
        public bool CheckListIsActive { get; set; }

        [NotMapped]
        public string SearchTerms { get; set; }
        public string BuildSearchTerms() => $"{CheckListName}".ToLower();

    }
}
