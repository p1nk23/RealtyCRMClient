using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Text.Json;
using RealtyCRMClient.DTOs;
using RealtyCRM.DTOs;

public class ApiService
{
    private readonly HttpClient _client;
    private const string BaseUrl = "https://localhost:5001/api/";

    public ApiService()
    {
        _client = new HttpClient();
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
                Status = c.Status
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





    // Создать новую карточку
    public async Task CreateCardAsync(CardObjectRieltyDto card)
    {
        var response = await _client.PostAsJsonAsync($"{BaseUrl}CardObjectRielty", card);
        response.EnsureSuccessStatusCode();
    }

    // Обновить карточку
    public async Task UpdateCardAsync(int id, CardObjectRieltyDto card)
    {
        var response = await _client.PutAsJsonAsync($"{BaseUrl}CardObjectRielty/{id}", card);
        response.EnsureSuccessStatusCode();
    }

    // Удалить карточку
    public async Task DeleteCardAsync(int id)
    {
        var response = await _client.DeleteAsync($"{BaseUrl}CardObjectRielty/{id}");
        response.EnsureSuccessStatusCode();
    }
}