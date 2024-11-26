using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data
{
    [Table("Party")]
    public partial class Party
    {
        [Key]
        public int PartyId { get; set; }
        public Guid PartyGuid { get; set; }
        public string PartySalutation { get; set; }
        public string PartyLastName { get; set; }
        public string PartyFirstName { get; set; }
        public string PartyAlias { get; set; }
        public string RelativeSalutation { get; set; }
        public string RelativeFirstName { get; set; }
        public string RelativeLastName { get; set; }
        public string ContactPerson { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string PinCode { get; set; }
        public string PhoneNoIsdCode { get; set; }
        public string PhoneNo { get; set; }
        public string email { get; set; }
        public string PAN { get; set; }
        public string AadharNo { get; set; }
        public bool IsPartyActive { get; set; }
        public string GSTIN { get; set; }
        public int? GroupId { get; set; }
        public string BankName { get; set; }
        public string Branch { get; set; }
        public string IFSCCode { get; set; }
        public string AccountNumber { get; set; }
        public int? SalutationType { get; set; }
        [NotMapped]
        public string GroupName { get; set; }
        [NotMapped]
        public string SearchTerms { get; set; }
        public string BuildSearchTerms() => $"{PartyId} {PartyFirstName} {PartyAlias} {email} {AddressLine1}".ToLower();
        [NotMapped]
        public  ICollection<PartyDocuments> PartyDocuments { get; set; }
    }
}
