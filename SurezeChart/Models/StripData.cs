using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SurezeChart.Models
{
    public class StripData
    {
        [Key]
        public int Id { get; set; }

        public int StripId { get; set; }

        public Double FHR1 { get; set; }
        public Double FHR2 { get; set; }
        public Double TOCO1 { get; set; }
        public Double SIGNAL1 { get; set; }
        public Double SPO { get; set; }

        [StringLength(200)]
        public string Raw { get; set; }

        [StringLength(5)]
        public string type { get; set; }

        //[StringLength(1)]
        //public String MigrationStatus { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDt { get; set; }

        //public string ChangedBy { get; set; }

        //public DateTime? ChangedDt { get; set; }
        //public IList<StripNotes> StripNotesDetails { get; set; }

        public int? StripNotes_Id { get; set; }

        [ForeignKey("StripNotes_Id")]
        public StripNotes StripNotes { get; set; }


    }
}
