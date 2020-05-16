using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HypestoreFinal.Models;

namespace HypestoreFinal.Models
{
    public class Cart
    {
        [Key]
        public string cart_id { get; set; }
        public DateTime date_created { get; set; }

        public virtual ICollection<CartItem> Cart_Items { get; set; }
    }
}