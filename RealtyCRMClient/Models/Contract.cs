using RealtyCRMClient.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtyCRMClient.Models
{
    public class Contract
    {
        public int Id { get; set; }
        public int CardObjectRieltyId { get; set; }
        public long ClientId { get; set; }
        public long AgentId { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }

        // Навигационные свойства
        public CardObjectRieltyDto CardObjectRielty { get; set; }
        public ClientDto Client { get; set; }
        public PersonalDto Agent { get; set; }
    }
}
