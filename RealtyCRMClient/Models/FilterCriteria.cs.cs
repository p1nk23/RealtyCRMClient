using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtyCRMClient.Models
{
    public class FilterCriteria
    {
        public string Description { get; set; }
        public string Title { get; set; }
        public string CeilingType { get; set; }
        public string WindowView { get; set; }
        public string Bathroom { get; set; }
        public string Balcony { get; set; }
        public string Address { get; set; }
        public string Price { get; set; }
        public string TotalArea { get; set; }
        public string Parking { get; set; }
        public string Heating { get; set; }
        public string GasSupply { get; set; }
        public long? SelectedClientId { get; set; } // ID клиента (Personal_id)
    }
}
