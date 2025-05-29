using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtyCRMClient.Models
{
    public class DocumentFields
    {
        // Продавец
        public string SellerName { get; set; } = string.Empty;
        public string SellerAddress { get; set; } = string.Empty;

        // Покупатель
        public string BuyerName { get; set; } = string.Empty;
        public string BuyerAddress { get; set; } = string.Empty;

        // Объект недвижимости
        public string Address { get; set; } = string.Empty;
        public string TotalArea { get; set; } = string.Empty;
        public string PriceNumeric { get; set; } = string.Empty;
        public string PriceInWords { get; set; } = string.Empty;

        // Условия
        public bool IsMarriedSeller { get; set; }
        public bool IsMarriedBuyer { get; set; }
        public bool NotarizedConsentProvided { get; set; }

        // Дополнительно
        public string BankCellLocation { get; set; } = string.Empty;
    }
}
