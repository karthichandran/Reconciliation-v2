using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Data
{
   public class PropertyParty
    {
        [Key]
        public int PropertyPartyId { get; set; }
        public Guid PropertyGuid { get; set; }
        public int PartyId { get; set; }
        public int PropertyId { get; set; }
        public bool? IsPrimaryParty { get; set; }
        public bool IsGroup { get; set; }

        [NotMapped]
        public string PartyName { get; set; }
    }
}
