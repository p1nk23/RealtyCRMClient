using RealtyCRMClient.AdminModule.Exceptions;
using RealtyCRMClient.AdminModule.Models;
using RealtyCRMClient.DTOs;
using RealtyCRMClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace RealtyCRMClient.AdminModule.Services
{
    public class EntityService
    {
        private readonly ApiService _apiService;
        private readonly ILogger _logger;

        public EntityService(ApiService apiService, ILogger logger)
        {
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Получает список записей для указанного типа сущности.
        /// </summary>
        /// <param name="entityType">Тип сущности.</param>
        /// <returns>Список записей в виде DataGridItem.</returns>
        /// <exception cref="ApiException">Выбрасывается при ошибке API.</exception>
        public async Task<List<DataGridItem>> GetEntitiesAsync(EntityType entityType)
        {
            _logger.Information("Получение списка сущностей типа {EntityType}", entityType);
            try
            {
                switch (entityType)
                {
                    case EntityType.Client:
                        var clients = await _apiService.GetAllClientsAsync();
                        return clients.Select(c => new DataGridItem
                        {
                            Id = c.Id,
                            DisplayText = c.Name ?? "Без имени",
                            EntityType = entityType
                        }).ToList();

                    case EntityType.Contract:
                        var contracts = await _apiService.GetAllContractsAsync();
                        return contracts.Select(c => new DataGridItem
                        {
                            Id = c.Id,
                            DisplayText = c.Description ?? "Без описания",
                            EntityType = entityType
                        }).ToList();

                    case EntityType.CardObjectRielty:
                        var cards = await _apiService.GetAllCardsAsync();
                        return cards.Select(c => new DataGridItem
                        {
                            Id = c.Id,
                            DisplayText = c.Title ?? "Без заголовка",
                            EntityType = entityType
                        }).ToList();

                    case EntityType.Comment:
                        var comments = await _apiService.GetAllAsync<CommentDto>(); // Предполагается, что метод добавлен в ApiService
                        return comments.Select(c => new DataGridItem
                        {
                            Id = c.Id,
                            DisplayText = c.Text ?? "Без текста",
                            EntityType = entityType
                        }).ToList();

                    case EntityType.DocumentTemplate:
                        var templates = await _apiService.GetAllTemplatesAsync();
                        return templates.Select(t => new DataGridItem
                        {
                            Id = t.Id,
                            DisplayText = t.Name ?? "Без имени",
                            EntityType = entityType
                        }).ToList();

                    case EntityType.Personal:
                        var personals = await _apiService.GetAllPersonalsAsync();
                        return personals.Select(p => new DataGridItem
                        {
                            Id = p.Id,
                            DisplayText = p.Name ?? "Без имени",
                            EntityType = entityType
                        }).ToList();

                    case EntityType.TaskObject:
                        var tasks = await _apiService.GetAllTasksAsync();
                        return tasks.Select(t => new DataGridItem
                        {
                            Id = t.Id,
                            DisplayText = t.Title ?? "Без заголовка",
                            EntityType = entityType
                        }).ToList();

                    default:
                        throw new ArgumentException("Неизвестный тип сущности", nameof(entityType));
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.Error(ex, "Ошибка при получении сущностей типа {EntityType}", entityType);
                throw new ApiException("Ошибка связи с сервером", (int?)ex.StatusCode ?? 500, ex);
            }
        }

        /// <summary>
        /// Удаляет запись по ID для указанного типа сущности.
        /// </summary>
        /// <param name="entityType">Тип сущности.</param>
        /// <param name="id">Идентификатор записи.</param>
        /// <exception cref="ApiException">Выбрасывается при ошибке API.</exception>
        public async Task DeleteEntityAsync(EntityType entityType, long id)
        {
            _logger.Information("Удаление сущности типа {EntityType} с ID {Id}", entityType, id);
            try
            {
                switch (entityType)
                {
                    case EntityType.Client:
                        await _apiService.DeleteAsync<ClientDto>(id);
                        break;
                    case EntityType.Contract:
                        await _apiService.DeleteAsync<ContractDto>(id);
                        break;
                    case EntityType.CardObjectRielty:
                        await _apiService.DeleteCardAsync((int)id);
                        break;
                    case EntityType.Comment:
                        await _apiService.DeleteAsync<CommentDto>(id);
                        break;
                    case EntityType.DocumentTemplate:
                        await _apiService.DeleteAsync<DocumentTemplateDto>(id);
                        break;
                    case EntityType.Personal:
                        await _apiService.DeleteAsync<PersonalDto>(id);
                        break;
                    case EntityType.TaskObject:
                        await _apiService.DeleteAsync<TaskObjectDto>(id);
                        break;
                    default:
                        throw new ArgumentException("Неизвестный тип сущности", nameof(entityType));
                }
                _logger.Information("Сущность типа {EntityType} с ID {Id} успешно удалена", entityType, id);
            }
            catch (HttpRequestException ex)
            {
                _logger.Error(ex, "Ошибка при удалении сущности типа {EntityType} с ID {Id}", entityType, id);
                throw new ApiException("Ошибка при удалении записи", (int?)ex.StatusCode ?? 500, ex);
            }
        }
    }
}
