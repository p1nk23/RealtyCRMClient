using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtyCRMClient.DTOs
{
    public class CreateCommentDto
    {
        [Required(ErrorMessage = "Текст комментария обязателен")]
        public string? Text { get; set; }

        public int? CardObjId { get; set; }
        public long? TaskObjId { get; set; }

        [Required(ErrorMessage = "Дата обязательна")]
        public DateTime? Time { get; set; }
    }
}
