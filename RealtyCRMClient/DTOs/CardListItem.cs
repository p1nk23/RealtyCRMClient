namespace RealtyCRM.DTOs
{
    public class CardListItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CeilingType { get; set; }
        public string WindowView { get; set; }
        public string Bathroom { get; set; }
        public string Balcony { get; set; }
        public string Address { get; set; }
        public string Price { get; set; }
        public string TotalArea { get; set; }
        public string Parking { get; set; }
        public string Heating { get; set; }
        public string GasSupply { get; set; }
        public long? Personal_id { get; set; }
        public int? Status { get; set; }
        public string StatusText
        {
            get
            {
                return Status switch
                {
                    0 => "Очередь",
                    1 => "В работе",
                    2 => "Ожидание ответа",
                    3 => "Готово",
                    4 => "Предпочтения клиента",
                    _ => "Очередь"
                };
            }
        }
    }
}
