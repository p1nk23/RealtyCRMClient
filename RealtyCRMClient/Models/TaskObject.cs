// Models/TaskObject.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealtyCRM.Models
{
    public class TaskObject
    {
        public long? Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long PersonalId { get; set; }

        //Навигационное свойство для связи с Personal
        public virtual Personal Personal { get; set; } // virtual для Lazy Loading

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}