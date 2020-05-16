using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypestoreFinal.Models
{
    public class Department
    {
        [Key]
        [Display(Name = "ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Department_ID { get; set; }
        [Required]
        [Display(Name = "Department Name")]
        [MinLength(3)]
        [MaxLength(80)]
        public string Department_Name { get; set; }

        [Display(Name = "Date Modified")]
        [DataType(DataType.Date)]
        public string Date_Modified { get; set; }

        public ICollection<Category> categories { get; set; }
    }
}