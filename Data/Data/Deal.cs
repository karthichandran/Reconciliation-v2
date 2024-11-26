using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data
{
    public class Deal
    {
        [Key]
        public int DealId { get; set; }
        public int PropertyMergeId { get; set; }
        public int CompanyId { get; set; }
        public decimal SaleValue1 { get; set; }
        public decimal SaleValue2 { get; set; }

        [NotMapped]
        public string SearchTerms { get; set; }

        public string BuildSearchTerms() => $"{DealName} ".ToLower();

        [NotMapped]
        public IList<DealParties> DealParties = new List<DealParties>();
        [NotMapped]
        public IList<DealPaySchedule> DealPaySchedules = new List<DealPaySchedule>();
        [NotMapped]
        public string DealName { get; set; }
        [NotMapped]
        public string Amount1 { get; set; }
        [NotMapped]
        public string Amount2 { get; set; }

    }
}
