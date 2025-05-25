using RealtyCRMClient.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtyCRMClient.Models
{
    public class ClientDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public int? CardObjId { get; set; }
        public long? TaskObjId { get; set; }

        // Связанный объект недвижимости
        public CardObjectRieltyDto CardObject { get; set; }
    }
    public class CreateClientDto
    {
        public string Name { get; set; }
        public string Number { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public int? CardObjId { get; set; }
        public long? TaskObjId { get; set; }

        // Связанный объект недвижимости
        public CardObjectRieltyDto CardObject { get; set; }
    }
    public class UpdateClientDto
    {

        public string Name { get; set; }
        public string Number { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public int? CardObjId { get; set; }
        public long? TaskObjId { get; set; }

    }
}
