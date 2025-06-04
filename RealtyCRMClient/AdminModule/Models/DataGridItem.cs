using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtyCRMClient.AdminModule.Models
{
    public class DataGridItem
    {
        /// <summary>
        /// Идентификатор записи.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Ключевые поля для отображения (например, имя, заголовок).
        /// </summary>
        public string DisplayText { get; set; }

        /// <summary>
        /// Тип сущности.
        /// </summary>
        public EntityType EntityType { get; set; }
    }
}
