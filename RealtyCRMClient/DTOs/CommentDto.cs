using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtyCRMClient.DTOs
{
    public class CommentDto
    {
        public long Id { get; set; }
        public string? Text { get; set; }
        public int? CardObjId { get; set; }
        public long? TaskObjId { get; set; }
        public DateTime Time { get; set; }
        public CardObjectRieltyDto? CardObject { get; set; }
        public TaskObjectDto? TaskObject { get; set; }
    }
}
