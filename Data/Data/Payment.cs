using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data
{
	public class Payment
    {
        public int PaymentId { get; set; }
        public Guid PaymentGuid { get; set; }
        public int PayeeId { get; set; }
        public int PayeeTypeId { get; set; }
        public int ExpenseHeadId { get; set; }
        public int PropertyId { get; set; }
        public int PartyId { get; set; }
        public int DocumentTypeId { get; set; }
        public int PaymentTypeId { get; set; }
        public DateTime DateOfPayment { get; set; }
        public decimal Amount { get; set; }
        public string ChequeNo { get; set; }
        public string Narration { get; set; }
        public int? CashAccountId { get; set; }
        public int? BankAccountId { get; set; }
        public int? GroupId { get; set; }
        public bool PDC { get; set; }

        [NotMapped]
        public List<PaymentList> PaymentLists { get; set; }


        [NotMapped]
        public string SearchTerms { get; set; }
        public string BuildSearchTerms() => $"{PayeeId} {ChequeNo} ".ToLower();
        [NotMapped]
        public string AccountName { get; set; }
        [NotMapped]
        public string CompanyName { get; set; }
        [NotMapped]
        public string PropertyName { get; set; }
        [NotMapped]
        public string DocumentTypeName { get; set; }


    }
}
