using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data
{
	public class PropertyMerge
    {
        public int PropertyMergeId { get; set; }
        public Guid PropertyMergeGuid { get; set; }
        public string PropertyMergeDealName { get; set; }
        public decimal MergedTotalArea { get; set; }
        public decimal MergedSaleValue1 { get; set; }
        public decimal MergedSaleValue2 { get; set; }
        public decimal MergedAmountPaid1 { get; set; }
        public decimal MergedAmountPaid2 { get; set; }
        public decimal MergedBalancePayable1 { get; set; }
        public decimal MergedBalancePayable2 { get; set; }
        public bool ForProposal { get; set; }
        public string FormattedTotalArea { get; set; }
        [NotMapped]
        public List<PropertyMergeList> propertyMergeLists { get; set; }
        [NotMapped]
        public bool IsSold { get; set; }

        [NotMapped]
        public string SearchTerms { get; set; }

        public string BuildSearchTerms() => $"{PropertyMergeDealName}".ToLower();
    }
}
