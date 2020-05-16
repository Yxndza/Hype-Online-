using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HypestoreFinal.Models
{
    public class OutofStockItems
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int ItemID { get; set; }

            [ForeignKey("items")]
            public int ItemCode { get; set; }

            public int SupplierId { get; set; }

            public string Name { get; set; }
        

            [Display(Name = "Price")]
            [DataType(DataType.Currency)]
            public double Price { get; set; }

            [Display(Name = "Re_Order Level")]
            public double ReOrder_Level { get; set; }


            [Display(Name = "Quantity in Stock")]
            public int QuantityInStock { get; set; }

            [Display(Name = "Re Order_Quantity")]
            public int ReOrder_Quantity { get; set; }

          
            public byte[] Image { get; set; }
            

            public virtual Item items { get; set; }

        
    }
}