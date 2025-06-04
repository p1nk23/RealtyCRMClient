using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtyCRMClient.AdminModule.Models
{
    /// <summary>
    /// Модель для представления сущности в ComboBox.
    /// </summary>
    public class EntityItem
    {
        /// <summary>
        /// Название сущности для отображения в UI.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Тип сущности (перечисление).
        /// </summary>
        public EntityType Type { get; set; }
    }

    /// <summary>
    /// Перечисление типов сущностей, поддерживаемых для удаления.
    /// </summary>
    public enum EntityType
    {
        Client,
        Contract,
        CardObjectRielty,
        Comment,
        DocumentTemplate,
        Personal,
        TaskObject
    }
}
