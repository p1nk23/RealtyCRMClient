using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtyCRMClient.DTOs
{
    public class CardObjectRieltyDto
    {
        public int Id { get; set; }
        public int? Status { get; set; }
        public string? Title { get; set; }
        public int? City { get; set; }
        public string? MetroNearby { get; set; }
        public string? Description { get; set; }
        public int? NumberOfRooms { get; set; }
        public string? CeilingType { get; set; }
        public float? CeilingHeight { get; set; }
        public string? WindowView { get; set; }
        public string? Renovation { get; set; }
        public string? Bathroom { get; set; }
        public string? Balcony { get; set; }
        public string? RoomsType { get; set; }
        public long? RosreestrCheck { get; set; }
        public string? Address { get; set; }
        public string? ListingId { get; set; }
        public string? Link { get; set; }
        public string? Price { get; set; }
        public string? Level { get; set; }
        public string? HousingType { get; set; }
        public string? LivingArea { get; set; }
        public string? SoldWithFurniture { get; set; }
        public string? ConstructionYear { get; set; }
        public string? HouseType { get; set; }
        public string? Parking { get; set; }
        public string? Entrances { get; set; }
        public string? Heating { get; set; }
        public string? EmergencyStatus { get; set; }
        public string? GasSupply { get; set; }
        public string? TotalArea { get; set; }
        public string? KitchenArea { get; set; }
        public long? Personal_id { get; set; }
        public long? Task_id { get; set; }

        public ICollection<CommentDto> Comments { get; set; } = new List<CommentDto>();
    }
}
