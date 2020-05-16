using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypestoreFinal.ViewModels
{
   public class Below_Order_Level_VM
    {
        [Display(Name = "Category")]
        public string Category { get; set; }
        [Display(Name = "Item Name")]
        public string Name { get; set; }
        public int   itemCode { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Stock Quantity")]
        public int stockQTY { get; set; }
        [Display(Name = "Image")]
        public byte[] Image { get; set; }
        [Display(Name = "Price")]
        public decimal Price { get; set; }
        public int ReOrderLevel { get; set; }
    }
}
