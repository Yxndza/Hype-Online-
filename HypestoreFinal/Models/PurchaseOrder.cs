using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HypestoreFinal.Models
{
    public class PurchaseOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PurchaseOrderID { get; set; }
       // public string ItemName { get; set; }


        public int SupplierID { get; set; }

        public string Email { get; set; }
       
       //public int ReorderQuantity { get; set; }
        public DateTime PurchaseDate { get; set; }

        public ICollection<PurchaseOrderItem> purchaseOrders { get; set; }
    }
}