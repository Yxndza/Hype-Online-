using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HypestoreFinal.Models
{
    public class PurchaseOrderItem
    {
       

        [Key]
        [Display(Name = "ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [ForeignKey("PurchaseOrder")]
        public int purchaseID { get; set; }
        public virtual PurchaseOrder PurchaseOrder{get; set;}

        public int SupplierId { get; set; }
        
        public int itemID { get; set; }
        public virtual Item Item { get; set; }
        public int reorderQuantity { get; set; }
        public string status { get; set; }

        //public virtual Supplier supplier { get; set; }

        
    }
}