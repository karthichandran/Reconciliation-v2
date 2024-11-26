using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data
{
	public class FundTransfer
    {
        [Key]
        public int FundTransferId { get; set; }
        public Guid FundTransferGuid { get; set; }
        public int PayeeId { get; set; }
        public int PayeePaymentType { get; set; }
        public int PayeeBankId { get; set; }
        public int PayeeCashId { get; set; }
        public DateTime DateOfPayment { get; set; }
        public decimal Amount { get; set; }
        public string Narration { get; set; }
        public int ReceiverId { get; set; }
        public int ReceiverPaymentType { get; set; }
        public int ReceiverBankId { get; set; }
        public int ReceiverCashId { get; set; }

        [NotMapped]
        public string SearchTerms { get; set; }
        public string BuildSearchTerms() => $"{PayeeId} {ReceiverId}".ToLower();
        [NotMapped]
        public string FromCompanyName { get; set; }
        [NotMapped]
        public string ToCompanyName { get; set; }
        [NotMapped]
        public string FromAccountName { get; set; }
        [NotMapped]
        public string ToAccountName { get; set; }

    }
}
