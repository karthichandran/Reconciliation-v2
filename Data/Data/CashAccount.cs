using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data
{
	public class CashAccount
    {
        [Key]
        public int CashAccountId { get; set; }
        public Guid CashAccountGuid { get; set; }
        public int AccountTypeId { get; set; }
        public int CompanyID { get; set; }
        public string CashAccountName { get; set; }
        public bool IsCashAccountActive { get; set; }
        public decimal? OpeningBalance { get; set; }

        [NotMapped]
        public string SearchTerms { get; set; }
        public string BuildSearchTerms() => $"{CashAccountName}".ToLower();

        [NotMapped]
        public string CompanyName { get; set; }

    }
}
