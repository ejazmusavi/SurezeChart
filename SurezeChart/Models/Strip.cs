using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurezeChart.Models
{
    public class Strip
    {
        public decimal Id { get; set; }
        public decimal CTGID { get; set; }
        public decimal PATIENTID { get; set; }
        public decimal SIGNAL { get; set; }
        public decimal HRA { get; set; }
        public decimal HRB { get; set; }
        public decimal TOCO { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
    