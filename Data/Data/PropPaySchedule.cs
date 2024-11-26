using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data
{
    public class PropPaySchedule
    { 
        [Key]
        public int PropPayScheduleId { get; set; }
        public int PropertyId { get; set; }
        public int PropertyDocumentTypeId { get; set; }
        public DateTime ScheduleDate { get; set; }
        public string Description { get; set; }
        public decimal Amount1 { get; set; }
        public decimal Amount2 { get; set; }
    }
}
