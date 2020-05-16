using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypestoreFinal.Models
{
    public class CartItem
    {
        [Key]
        public string cartitemid { get; set; }
        [ForeignKey("Cart")]
        public string cartid { get; set; }
        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }

        public virtual Item Item { get; set; }
        public virtual Cart Cart { get; set; }
    }
}