using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SurezeChart.Models
{
    public class StripNotes
    {
        [Key]
        public int Id { get; set; }

        public int StripDataId { get; set; }

        [ForeignKey("StripDataId")]
        public StripData StripData { get; set; }

        [StringLength(250)]
        public string Notes { get; set; }

        [StringLength(1)]
        public String MigrationStatus { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDt { get; set; }

        public string ChangedBy { get; set; }

        public DateTime? ChangedDt { get; set; }
    }
}
