using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealtyCRM.Models
{
    public class Comment
    {
        public long Id { get; set; }
        public string? Text { get; set; }
        public int? CardObjId { get; set; } // Может быть null
        public long? TaskObjId { get; set; } // Может быть null
        public DateTime Time { get; set; }
        public CardObjectRielty? CardObject { get; set; }
        public TaskObject? TaskObject { get; set; }
    }
}
