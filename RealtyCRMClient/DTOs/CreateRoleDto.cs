using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtyCRMClient.DTOs
{
    public class CreateRoleDto
    {
        [Required(ErrorMessage = "Название роли обязательно")]
        public string? Title { get; set; }
    }
}
