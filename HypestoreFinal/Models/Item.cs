using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypestoreFinal.Models
{
    public class Item
    {
      

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name="Item Code")]
        public int ItemCode { get; set; }

        
        [Display(Name = "Item #")]
        public int ItemId { get; set; }

        [ForeignKey("Categories")]
        [Display(Name = "Category")]
        public int Category_ID { get; set; }

        [Display(Name = "Brand")]
        public Enumerations.Brand Brand { get; set; }

        [Display(Name="Supplied By")]
        [ForeignKey("suppliers")]
        public int SupplierId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        [Display(Name = "Size")]
        public Enumerations.Size size { get; set; }

        [Display(Name = "Color")]
        public Enumerations.color color { get; set; }

        [Display(Name = "Style No.")]
        public string SkuNo { get; set; }

        [Display(Name = "Price")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        //[Required]
        //[Display(Name = "Mark-Up Percentage")]
        //public double Mark_Up { get; set; }


        //[Required]
        [Display(Name = "Re_Order Level")]
        public double ReOrder_Level { get; set; }


        [Display(Name = "Quantity in Stock")]
        public int QuantityInStock { get; set; }

        [Display(Name = "Re Order_Quantity")]
        public int ReOrder_Quantity { get; set; }

        [Display(Name = "Re-Order Percentage")]
        public double ReOrder_Percent { get; set; }
        public byte[] Image { get; set; }
        

        [Display(Name = "Is Discontinued?")]
        public bool isDiscontinued { get; set; }

        //[Display(Name = "Discontinue Reason")]
        //public Enums.Discontinue_reason Discontinue_Reason { get; set; }

        //[Display(Name = "Discontinue Date")]
        //[DataType(DataType.Date)]
        //public DateTime? Discontinue_Date { get; set; }

        //[Display(Name = "Date Modified")]
        //[DataType(DataType.Date)]
        //public DateTime? Date_Modified { get; set; }

        [ForeignKey("Categories")]
        [Display(Name = "Category")]
        public virtual Category Categories { get; set; }
      public virtual Supplier suppliers { get; set; }
        

        public ICollection<OutofStockItems> outofStocks { get; set; }
        //public virtual Brand Brands { get; set; }
        //public virtual Size Sizes { get; set; }
        //public virtual Color Colors { get; set; }
    }
}