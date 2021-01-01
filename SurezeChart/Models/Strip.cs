using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurezeChart.Models
{
    public class Strip
    {
        public decimal Id { get; set; }
        public decimal PATIENTID { get; set; }
        public decimal SIGNAL { get; set; }
        public double HRA { get; set; }
        public double HRB { get; set; }
        public double TOCO { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
    