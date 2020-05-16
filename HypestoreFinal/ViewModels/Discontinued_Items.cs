using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypestoreFinal.ViewModels
{
   public class Discontinued_Items
    {
        [Display(Name ="Category Name")]
        public string Cat_Name { get; set; }

        [Display(Name ="Item Name")]
        public string item_Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }
       
        [Display(Name = "Picture")]
        public byte[] Picture { get; set; }

        [Display(Name ="Discontinue Date")]
        public string disc_Date { get; set; }

        [Display(Name = "Discontinue Reason")]
        public string Disc_Reason { get; set; }
    }
}
