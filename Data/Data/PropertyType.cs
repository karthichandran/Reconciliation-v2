using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data 
{
    public class PropertyType
    {
        public int PropertyTypeId { get; set; }
        public Guid PropertyTypeGuid { get; set; }
        public string PropertyTypeText { get; set; }
        public bool PropertyTypeIsActive { get; set; }

        [NotMapped]
        public string SearchTerms { get; set; }
        public string BuildSearchTerms() => $"{PropertyTypeText}".ToLower();
    }
}
