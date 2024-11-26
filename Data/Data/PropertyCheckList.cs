using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data
{
    public class PropertyCheckList
    {
        [Key]
        public int PropertyCheckListId { get; set; }

        public int CompanyID { get; set; }
        public Guid PropertyGuid { get; set; }
        public string PropertyName { get; set; }
        public int TalukId { get; set; }
        public int HobliId { get; set; }
        public int VillageId { get; set; }
        public int DocumentTypeId { get; set; }
        public int PropertyTypeId { get; set; }
        public string SurveyNo { get; set; }
        public string PropertyGMapLink { get; set; }
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
        public bool CheckListMaster { get; set; }
        public string PropertyDescription { get; set; }
        public int Status { get; set; }
        public string Remarks { get; set; }      
        [NotMapped]
        public string SearchTerms { get; set; }

        [NotMapped]
        public IList<PropertyCheckListDocuments> PropertyCheckListDocuments { get; set; }
        [NotMapped]
        public IList<CheckListOfProperty> CheckListOfProperties { get; set; }
        [NotMapped]
        public IList<PropertyCheckListVendor> PropertyCheckListVendors { get; set; }
        [NotMapped]
        public string CompanyName { get; set; }
        [NotMapped]
        public string VillageName { get; set; }
        [NotMapped]
        public string TotalArea { get; set; }

        public string BuildSearchTerms() => $"{CompanyName} {SurveyNo} {PropertyName} ".ToLower();
    }
}
