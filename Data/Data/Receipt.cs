using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data
{
	public class Receipt
    {
        [Key]
        public int ReceiptId { get; set; }
        public Guid ReceiptGuid { get; set; }
        public int PayeeId { get; set; }
        public int DealId { get; set; }
        public int PartyId { get; set; }
        public int PaymentTypeId { get; set; }
        public int DepositBankId { get; set; }
        public int DepositCashId { get; set; }
        public DateTime DateOfPayment { get; set; }
        public decimal Amount { get; set; }
        public string Narration { get; set; }
        
        [NotMapped]
        public string SearchTerms { get; set; }

        public string BuildSearchTerms() => $"{BankName}".ToLower();
        [NotMapped]
        public string BankName { get; set; } 
        [NotMapped]
        public string CashName { get; set; }
    }
}
