using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RealtyCRM.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string? Title { get; set; }
    }
}
