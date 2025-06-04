namespace RealtyCRMClient.Models
{
    /// <summary>
    /// Перечисление статусов для договоров и задач.
    /// </summary>
    public enum StatusType
    {
        Queue = 0,        // Очередь
        InProgress = 1,   // В работе
        AwaitingResponse = 2, // Ожидание ответа
        Completed = 3     // Готово
    }
}