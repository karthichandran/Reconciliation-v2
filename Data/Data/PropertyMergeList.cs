using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data
{
	public class PropertyMergeList
    {
        public int PropertyMergeListId { get; set; }
        public Guid PropertyMergeGuid { get; set; }
        public Guid PropertyGuid { get; set; }
        public int PropertyDocumentTypeId { get; set; }

        [NotMapped]
        public string PropertyName { get; set; }
        [NotMapped]
        public string DocumentTypeName { get; set; }
        [NotMapped]
        public string Party { get; set; }
        [NotMapped]
        public string Village { get; set; }
        [NotMapped]
        public string SurveyNo { get; set; }
        [NotMapped]
        public string LandArea { get; set; }
        [NotMapped]
        public string AKarab { get; set; }
        [NotMapped]
        public string BKarab { get; set; }
        [NotMapped]
        public string SaleValue1 { get; set; }
        [NotMapped]
        public string SaleValue2 { get; set; }
        [NotMapped]
        public string Amount1 { get; set; }
        [NotMapped]
        public string Amount2 { get; set; }
        [NotMapped]
        public string TotalArea { get; set; }
        [NotMapped]
        public string Expense { get; set; }
        [NotMapped]
        public string Balance1 { get; set; } 
        [NotMapped]
        public string Balance2 { get; set; }
        [NotMapped]
        public string companyId { get; set; }
        [NotMapped]
        public string propertyId { get; set; }
        [NotMapped]
        public bool IsOld { get; set; }
    }
}
