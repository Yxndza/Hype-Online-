using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using HypestoreFinal.Models;

namespace HypestoreFinal.Models
{
    public class Category
    {

        [Key]
        [Display(Name = "ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Category_ID { get; set; }

        [Required]
        [ForeignKey("Departments")]
        [Display(Name = "Department")]
        public int Department_ID { get; set; }


        [Required]
        [Display(Name = "Name")]
        [MinLength(3)]
        [MaxLength(80)]
        public string Category_Name { get; set; }

        [Required]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        [MinLength(3)]
        [MaxLength(255)]
        public string Description { get; set; }


        [Required]
        [Display(Name = "Safety Stock Level")]
        public int Safety_Stock_level { get; set; }



        [Display(Name = "Date Modified")]
        [DataType(DataType.Date)]
        public string Date_Modified { get; set; }

        public virtual Department Departments { get; set; }

        public virtual ICollection<Item> Items { get; set; }
       // public virtual ICollection<ItemType> ItemTypes { get; set; }
    }
}