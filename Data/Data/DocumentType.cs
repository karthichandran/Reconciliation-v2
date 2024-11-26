using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data
{
    public class DocumentType
    {
        [Key]
        public int DocumentTypeId { get; set; }
        public string DocumentTypeName { get; set; }
        public bool IsDocumentTypeActive { get; set; }

        [NotMapped]
        public string SearchTerms { get; set; }
        public string BuildSearchTerms() => $"{DocumentTypeName}".ToLower();
    }
}
