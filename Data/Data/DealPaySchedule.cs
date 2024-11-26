using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data
{
   public class DealPaySchedule
    {
        [Key]
        public int DealPayScheduleId { get; set; }
        public int DealId { get; set; }
        public DateTime ScheduleDate { get; set; }
        public string Description { get; set; }
        public decimal Amount1 { get; set; }
        public decimal Amount2 { get; set; }
    }
}
