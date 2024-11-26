using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class PropertyCostDetails
    {
        public int PropertyId { get; set; }
        public int PropertyDocumentTypeId { get; set; }
        public string ComapnyName { get; set; }
        public string PropertyName { get; set; }
        public string PropertyType { get; set; }
        public string Taluk { get; set; }
        public string Hobli { get; set; }
        public string Village { get; set; }
        public string SurveyNo { get; set; }
        public string DocumentType { get; set; }
        public decimal SaleValue1 { get; set; }
        public decimal SaleValue2 { get; set; }
        public List<PropertyParty> PropertyParties { get; set; }
        public List<PropPaySchedule> PropPaySchedules { get; set; }
    }
}
