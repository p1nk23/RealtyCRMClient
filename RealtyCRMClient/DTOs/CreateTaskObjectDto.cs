using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtyCRMClient.DTOs
{
    public class CreateTaskObjectDto
    {
        [Required(ErrorMessage = "Название задачи обязательно")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Описание обязательно")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Дата начала обязательна")]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "ID сотрудника обязателен")]
        public long PersonalId { get; set; }
    }
}
