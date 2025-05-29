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


public class ApiService
{
    private readonly HttpClient _client;
    private const string BaseUrl = "https://localhost:5001/api/";

    public ApiService()
    {
        _client = new HttpClient();
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
        var response = await _client.DeleteAsync($"{BaseUrl}CardObjectRielty/{id}");
        response.EnsureSuccessStatusCode();
    }
}