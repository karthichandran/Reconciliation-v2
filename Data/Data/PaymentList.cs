using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data
{
	public class PaymentList
    {
        public int PaymentListId { get; set; }
        public int PaymentId { get; set; }
        public DateTime DateOfPayment { get; set; }
        public decimal Amount { get; set; }
        public string ChequeNo { get; set; }
        public string Narration { get; set; }
        public bool PDC { get; set; }
        public bool PaymentTypeId { get; set; }
        public int CashAccountId { get; set; }
        public int BankAccountId { get; set; }
        [NotMapped]
        public string AccountName { get; set; }
    }
}
