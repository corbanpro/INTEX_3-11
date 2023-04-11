using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace INTEX_3_11.Models
{
    public class NewEntry
    {
        [Key]

        [Required(ErrorMessage = "Sex is required!")]
        public string Sex { get; set; }
        public string Color { get; set; }
        public string Structure { get; set; }
        [Required(ErrorMessage = "Depth is required!")]
        public double Depth { get; set; }
        [Required(ErrorMessage = "Age at death is required!")]
        public string Ageatdeath { get; set; }
        public string Headdirection { get; set; }
        public string Textilefunction { get; set; }
        [Required(ErrorMessage = "Hair color is required!")]
        public string Haircolor { get; set; }

        [MaxLength(100)]
        public string Text { get; set; }

    }
}
