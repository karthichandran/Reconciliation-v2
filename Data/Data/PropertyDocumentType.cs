using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data
{
    public class PropertyDocumentType
    {
        [Key]
        public int PropertyDocumentTypeId { get; set; }
        public int PropertyId { get; set; }               
        public int DocumentTypeId { get; set; }        
        public decimal LandAreaInputAcres { get; set; }
        public decimal LandAreaInputGuntas { get; set; }
        public decimal LandAreaInputAanas { get; set; }
        public decimal LandAreaInAcres { get; set; }
        public decimal LandAreaInGuntas { get; set; }
        public decimal LandAreaInSqMts { get; set; }
        public decimal LandAreaInSqft { get; set; }
        public decimal AKarabAreaInputAcres { get; set; }
        public decimal AKarabAreaInputGuntas { get; set; }
        public decimal AKarabAreaInputAanas { get; set; }
        public decimal AKarabAreaInAcres { get; set; }
        public decimal AKarabAreaInGuntas { get; set; }
        public decimal AKarabAreaInSqMts { get; set; }
        public decimal AKarabAreaInSqft { get; set; }
        public decimal BKarabAreaInputAcres { get; set; }
        public decimal BKarabAreaInputGuntas { get; set; }
        public decimal BKarabAreaInputAanas { get; set; }
        public decimal BKarabAreaInAcres { get; set; }
        public decimal BKarabAreaInGuntas { get; set; }
        public decimal BKarabAreaInSqMts { get; set; }
        public decimal BKarabAreaInSqft { get; set; }
        public decimal SaleValue1 { get; set; }
        public decimal SaleValue2 { get; set; }
        [NotMapped]
        public string DocumentTypeName { get; set; }
        [NotMapped]
        public string LandArea { get; set; }
        [NotMapped]
        public IList<PropertyDocuments> PropertyDocuments { get; set; }
    }
}
