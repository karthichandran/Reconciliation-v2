using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data
{
	public class Taluk
    {
        [Key]
        public int TalukId { get; set; }
        public Guid TalukGuid { get; set; }
        public string TalukName { get; set; }
        public string TalukGMapLink { get; set; }
        public bool TalukIsActive { get; set; }

        [NotMapped]
        public string SearchTerms { get; set; }
        public string BuildSearchTerms() => $"{TalukName}".ToLower();

    }
}
