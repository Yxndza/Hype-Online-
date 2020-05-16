using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HypestoreFinal.ViewModels
{
    public class SummaryVM
    {
        public string CustomerName { get; set; }
        public string PetName { get; set; }
        public DateTime VisitDate { get; set; }
        public string Diagnosis { get; set; }
        public string ItemName { get; set; }
        public double Price { get; set; }
    }
}