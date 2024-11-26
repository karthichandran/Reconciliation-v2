using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data
{
    public class ScreenList
    {
        public int ModuleId { get; set; }
        public string ScreenName { get; set; }
        [Key]
        public int ScreenId { get; set; }
    }
}
