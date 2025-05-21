// Models/Personal.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealtyCRM.Models
{
    public class Personal
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public int? Role { get; set; } // Ссылается на таблицу roles
        public string? Login { get; set; }
        public string? Password { get; set; }
    }
}