using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HypestoreFinal.Models;

namespace HypestoreFinal.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string OrderID { get; set; }

        public DateTime date_created { get; set; }
        [ForeignKey("Customer")]
        public int customerId { get; set; }
        public string Email { get; set; }
        public bool shipped { get; set; }
        public string status { get; set; }
        public bool packed { get; set; }
        public virtual ICollection<OrderItem> Order_Items { get; set; }
       
        public virtual Customer Customer { get; set; }

        public virtual ICollection<OrderTracking> Order_Tracking { get; set; }
    }
}