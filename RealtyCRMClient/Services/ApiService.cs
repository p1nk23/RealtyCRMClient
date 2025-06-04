using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Text.Json;
using RealtyCRMClient.DTOs;
using RealtyCRM.DTOs;
using RealtyCRM.Models;
using RealtyCRMClient.Models;
using CardObjectRieltyDto = RealtyCRMClient.DTOs.CardObjectRieltyDto;
using Newtonsoft.Json;
using System.Text;
using Serilog;
using System.Net;
using RealtyCRMClient.AdminModule.Exceptions;


public class ApiService
{
    private readonly HttpClient _client;
    private readonly ILogger _logger;
    private const string BaseUrl = "https://localhost:5001/api/";

    public ApiService()
    {
        _client = new HttpClient();
    }
    public ApiService(ILogger logger)
    {
        _client = new HttpClient();
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public async Task<List<T>> GetAllAsync<T>()
    {
        string controllerName = GetControllerName<T>();
        _logger.Information("Получение всех записей для {ControllerName}", controllerName);
        try
        {
            var response = await _client.GetAsync($"{BaseUrl}{controllerName}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<T>>();
        }
        catch (HttpRequestException ex)
        {
            _logger.Error(ex, "Ошибка при получении записей для {ControllerName}", controllerName);
            throw new ApiException($"Ошибка связи с сервером: {ex.Message}", (int?)ex.StatusCode ?? 500, ex);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Неизвестная ошибка при получении записей для {ControllerName}", controllerName);
            throw new ApiException("Неизвестная ошибка при обработке ответа", 500, ex);
        }
    }
    public async Task DeleteAsync<T>(long id)
    {
        string controllerName = GetControllerName<T>();
        string url = $"{BaseUrl}{controllerName}/{id}";
        _logger.Information("Отправка DELETE-запроса для {ControllerName} с ID {Id}: {Url}", controllerName, id, url);
        try
        {
            var response = await _client.DeleteAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                _logger.Error("Ошибка при удалении {ControllerName} с ID {Id}. Статус: {StatusCode}, Ответ: {ResponseBody}",
                    controllerName, id, response.StatusCode, responseBody);
                string errorMessage = ExtractErrorMessage(responseBody, response.StatusCode);
                throw new ApiException(errorMessage, (int)response.StatusCode, null);
            }
            response.EnsureSuccessStatusCode();
            _logger.Information("Запись для {ControllerName} с ID {Id} успешно удалена", controllerName, id);
        }
        catch (HttpRequestException ex)
        {
            _logger.Error(ex, "Ошибка HTTP при удалении записи для {ControllerName} с ID {Id}", controllerName, id);
            throw new ApiException($"Ошибка при удалении: {ex.Message}", (int?)ex.StatusCode ?? 500, ex);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Неизвестная ошибка при удалении записи для {ControllerName} с ID {Id}", controllerName, id);
            throw new ApiException("Неизвестная ошибка при удалении", 500, ex);
        }
    }
    private string GetControllerName<T>()
    {
        string controllerName = typeof(T) switch
        {
            Type t when t == typeof(ClientDto) => "Client",
            Type t when t == typeof(ContractDto) => "Contract",
            Type t when t == typeof(CardObjectRieltyDto) => "CardObjectRielty",
            Type t when t == typeof(CommentDto) => "Comment",
            Type t when t == typeof(DocumentTemplateDto) => "DocumentTemplate",
            Type t when t == typeof(PersonalDto) => "Personal",
            Type t when t == typeof(TaskObjectDto) => "TaskObject",
            _ => throw new ArgumentException($"Неизвестный тип DTO: {typeof(T).Name}")
        };
        _logger.Information("Определено имя контроллера для типа {Type}: {ControllerName}", typeof(T).Name, controllerName);
        return controllerName;
    }

    private string ExtractErrorMessage(string responseBody, HttpStatusCode statusCode)
    {
        try
        {
            if (string.IsNullOrEmpty(responseBody))
                return $"Сервер вернул ошибку: {statusCode}";

            using var document = JsonDocument.Parse(responseBody);
            if (document.RootElement.TryGetProperty("message", out var messageElement))
            {
                return messageElement.GetString() ?? $"Сервер вернул ошибку: {statusCode}";
            }
            if (document.RootElement.TryGetProperty("error", out var errorElement))
            {
                return errorElement.GetString() ?? $"Сервер вернул ошибку: {statusCode}";
            }
            return responseBody;
        }
        catch
        {
            return $"Сервер вернул ошибку: {statusCode} - {responseBody}";
        }
    }







    public async Task<CardObjectRieltyDto> GetCardByClientIdAsync(long personalId)
    {
        try
        {
            var response = await _client.GetAsync($"{BaseUrl}CardObjectRielty?Personal_id={personalId}");
            response.EnsureSuccessStatusCode();
            var cards = await response.Content.ReadFromJsonAsync<List<CardObjectRieltyDto>>();
            return cards.FirstOrDefault();
        }
        catch
        {
            return null;
        }
    }


    // Контракты
    public async Task<List<ContractDto>> GetAllContractsAsync()
    {
        var response = await _client.GetAsync($"{BaseUrl}Contract");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<ContractDto>>();
    }

    public async Task<ContractDto> GetContractByIdAsync(int id)
    {
        var response = await _client.GetAsync($"{BaseUrl}Contract/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ContractDto>();
    }

    public async Task CreateContractAsync(ContractDto dto)
    {
        var json = JsonConvert.SerializeObject(dto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync($"{BaseUrl}Contract", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateContractAsync(int id, ContractDto dto)
    {
        var json = JsonConvert.SerializeObject(dto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PutAsync($"{BaseUrl}Contract/{id}", content);
        response.EnsureSuccessStatusCode();
    }

    //Задачи
    public async Task<List<TaskObjectDto>> GetAllTasksAsync()
    {
        var response = await _client.GetAsync($"{BaseUrl}TaskObject");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<TaskObjectDto>>();
    }

    public async Task<TaskObjectDto> GetTaskByIdAsync(long id)
    {
        var response = await _client.GetAsync($"{BaseUrl}TaskObject/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TaskObjectDto>();
    }

    public async Task UpdateTaskAsync(long id, TaskObjectDto dto)
    {
        var json = JsonConvert.SerializeObject(dto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PutAsync($"{BaseUrl}TaskObject/{id}", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task<PersonalDto> GetPersonalById(long id)
    {
        var response = await _client.GetAsync($"{BaseUrl}Personal/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<PersonalDto>();
    }





    public async Task CreateTaskAsync(CreateTaskObjectDto dto)
    {
        var json = JsonConvert.SerializeObject(dto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync($"{BaseUrl}TaskObject", content);
        response.EnsureSuccessStatusCode();
    }

    


    //Клиенты

    public async Task<ClientDto> GetClientByIdAsync(long id)
    {
        var response = await _client.GetAsync($"{BaseUrl}Client/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ClientDto>();
    }

    public async Task CreateClientAsync(CreateClientDto dto)
    {
        var json = JsonConvert.SerializeObject(dto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync($"{BaseUrl}Client", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateClientAsync(long id, UpdateClientDto dto)
    {
        var json = JsonConvert.SerializeObject(dto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PutAsync($"{BaseUrl}Client/{id}", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<ClientDto>> GetAllClientsAsync()
    {
        var response = await _client.GetAsync($"{BaseUrl}Client");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<ClientDto>>();
    }

    //Персонал организации
    public async Task<List<PersonalDto>> GetAllPersonalsAsync()
    {
        var response = await _client.GetAsync($"{BaseUrl}Personal");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<PersonalDto>>();
    }

    // Получить все карточки недвижимости 
    public async Task<List<CardListItem>> GetAllCardsAsync()
    {
        try
        {
            var response = await _client.GetAsync($"{BaseUrl}CardObjectRielty");
            response.EnsureSuccessStatusCode();
            var cards = await response.Content.ReadFromJsonAsync<List<CardObjectRieltyDto>>();
            return cards.Select(c => new CardListItem
            {
                Id = c.Id,
                Title = c.Title,
                Status = c.Status,
                Address = c.Address,
                Description = c.Description,
                CeilingType = c.CeilingType,
                WindowView = c.WindowView,
                Bathroom = c.Bathroom,
                Balcony = c.Balcony,
                Price = c.Price,
                TotalArea = c.TotalArea,
                Parking = c.Parking,
                Heating = c.Heating,
                GasSupply = c.GasSupply
            }).ToList();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: {ex.Message}");
            return new List<CardListItem>();
        }
    }
    public async Task<CardObjectRieltyDto> GetCardWithCommentsAsync(int id)
    {
        try
        {
            var response = await _client.GetAsync($"{BaseUrl}CardObjectRielty/{id}/with-comments");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CardObjectRieltyDto>();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: {ex.Message}");
            return null;
        }
    }
    public async Task<CardObjectRieltyDto> GetCardByIdAsync(int id)
    {
        try
        {
            var response = await _client.GetAsync($"{BaseUrl}CardObjectRielty/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CardObjectRieltyDto>();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: {ex.Message}");
            return null;
        }
    }

    // Получить все шаблоны документов
    public async Task<List<DocumentTemplateDto>> GetAllTemplatesAsync()
    {
        var response = await _client.GetAsync($"{BaseUrl}DocumentTemplate");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<DocumentTemplateDto>>();
    }

    // Получить шаблон по ID
    public async Task<DocumentTemplateDto> GetTemplateByIdAsync(int id)
    {
        var response = await _client.GetAsync($"{BaseUrl}DocumentTemplate/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<DocumentTemplateDto>();
    }



    // Создать новую карточку недвижимости
    public async Task CreateCardAsync(CardObjectRieltyDto card)
    {
        var response = await _client.PostAsJsonAsync($"{BaseUrl}CardObjectRielty", card);
        response.EnsureSuccessStatusCode();
    }

    // Обновить карточку недвижимости
    public async Task UpdateCardAsync(int id, CardObjectRieltyDto card)
    {
        var response = await _client.PutAsJsonAsync($"{BaseUrl}CardObjectRielty/{id}", card);
        response.EnsureSuccessStatusCode();
    }

    // Удалить карточку недвижимости
    public async Task DeleteCardAsync(int id)
    {
        try
        {
            var response = await _client.DeleteAsync($"{BaseUrl}CardObjectRielty/{id}");
            response.EnsureSuccessStatusCode();
        }
        catch
        {
            MessageBox.Show("Перед удалением объекта удалите все связанные с ним сущности");
        }
    }




}