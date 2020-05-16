using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypestoreFinal.Models
{
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderItemId { get; set; }
        [ForeignKey("Order")]
        public string OrderId { get; set; }
        public virtual Order Order { get; set; }

        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public virtual Item Item { get; set; }

        [Display(Name = "Price")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        public string StockNo { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Returned { get; set; }
        public byte[] Image { get; set; }
        //public virtual Inventory Inventories { get; set; }
        //public virtual Order Order { get; set; }
    }
}