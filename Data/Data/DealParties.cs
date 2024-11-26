using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data
{
    public class DealParties
    {
        [Key]
        public int DealPartyId { get; set; }
        public int DealId { get; set; }
        public int PartyId { get; set; }
        public bool IsGroup { get; set; }
        [NotMapped]
        public string PartyName { get; set; }
    }
}
