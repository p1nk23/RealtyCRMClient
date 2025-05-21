using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtyCRMClient.DTOs
{
    public class UpdateCommentDto
    {
        public string? Text { get; set; }
        public int? CardObjId { get; set; }
        public long? TaskObjId { get; set; }
        public DateTime? Time { get; set; }
    }
}
