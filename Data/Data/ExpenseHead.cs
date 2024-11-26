using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data
{
    [Table("ExpenseHead")]
    public partial class ExpenseHead
    {
        [Key]
        public int ExpenseHeadId { get; set; }
        public Guid ExpenseHeadGuid { get; set; }
        public string ExpenseHeadName { get; set; }
        public bool IsExpenseHeadActive { get; set; }
        [NotMapped]
        public string SearchTerms { get; set; }

        public string BuildSearchTerms() => $"{ExpenseHeadName}".ToLower();
    }
}
