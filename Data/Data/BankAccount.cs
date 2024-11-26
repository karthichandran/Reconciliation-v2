using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace Data
{
	public class BankAccount
    {
        [Key]
        public int BankAccountId { get; set; }
        public Guid BankGuid { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string AccountNumber { get; set; }
        public int AccountTypeId { get; set; }
        public string IFSCCode { get; set; }
        public decimal OpeningBalance { get; set; }
        public bool IsBankAccountActive { get; set; }
        public int CompanyID { get; set; }

        [NotMapped]
        public string SearchTerms { get; set; }
        public string BuildSearchTerms() => $"{BankName} {BranchName} {IFSCCode} {AccountNumber}".ToLower();
        [NotMapped]
        public string CompanyName { get; set; }
        [NotMapped]
        public string AccountTypeName { get; set; }
    }
}
