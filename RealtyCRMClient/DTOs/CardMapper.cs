using RealtyCRM.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtyCRMClient.DTOs
{
    public static class CardMapper
    {
        public static CardObjectRieltyDto MapToCardObjectRieltyDto(CardListItem item)
        {
            return new CardObjectRieltyDto
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                CeilingType = item.CeilingType,
                WindowView = item.WindowView,
                Bathroom = item.Bathroom,
                Balcony = item.Balcony,
                Address = item.Address,
                Price = item.Price,
                TotalArea = item.TotalArea,
                Parking = item.Parking,
                Heating = item.Heating,
                GasSupply = item.GasSupply,
                Personal_id = item.Personal_id,
                Status = item.Status
            };
        }
    }
}
