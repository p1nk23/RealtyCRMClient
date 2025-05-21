namespace RealtyCRM.DTOs
{
    public class CardListItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? Status { get; set; }

        // Конвертируем статус в текст
        public string StatusText
        {
            get
            {
                switch (Status)
                {
                    case 0:
                        return "Очередь";
                    case 1:
                        return "В работе";
                    case 2:
                        return "Ожидание ответа";
                    case 3:
                        return "Готово";
                    default:
                        return "Очередь";
                }
            }
        }
    }
}
