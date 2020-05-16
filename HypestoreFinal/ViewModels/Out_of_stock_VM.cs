using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypestoreFinal.ViewModels
{
    public class Out_of_stock_VM
    {
        [Display(Name = "Category")]
        public string Category { get; set; }
        [Display(Name = "Item Name")]
        public string Name { get; set; }

        public int itemCode { get; set; }
            [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Reorder Quantity")]
        public int stockQTY { get; set; }
        public decimal Price { get; set; }
        [Display(Name = "Image")]
        public byte[] Image { get; set; }

    }
}
