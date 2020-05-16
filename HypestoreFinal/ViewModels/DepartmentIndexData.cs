using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HypestoreFinal.Models;
namespace HypestoreFinal.ViewModels
{
    public class DepartmentIndexData
    {
        public IEnumerable<Department> departments { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Item> Items { get; set; }
    }
}