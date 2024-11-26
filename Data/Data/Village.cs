using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data
{
	public class Village
    {
        [Key]
        public int VillageId { get; set; }
        public Guid VillageGuid { get; set; }
        public int TalukId { get; set; }
        public int HobliId { get; set; }
        public string VillageName { get; set; }
        public string VillageGMapLink { get; set; }
        public bool VillageIsActive { get; set; }

        [NotMapped]
        public string SearchTerms { get; set; }
        public string BuildSearchTerms() => $"{TalukId} {HobliId} {VillageName}".ToLower();
        [NotMapped]
        public string TalukName { get; set; }
        [NotMapped]
        public string HobliName { get; set; }

    }
}
