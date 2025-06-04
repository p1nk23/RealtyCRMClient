using RealtyCRMClient.DTOs;
using RealtyCRMClient.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealtyCRMClient.Services
{
    public class StatisticsService
    {
        private readonly ApiService _apiService;
        private static readonly Dictionary<string, string> StatusDisplayNames = new Dictionary<string, string>
        {
            { "0", "Очередь" },
            { "1", "В работе" },
            { "2", "Ожидание ответа" },
            { "3", "Готово" }
        };
        private static readonly List<string> DefaultStatuses = new List<string> { "0", "1", "2", "3" };

        public StatisticsService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<List<ContractStatistics>> GetContractStatisticsAsync(string status = null)
        {
            var contracts = await _apiService.GetAllContractsAsync();
            var filteredContracts = string.IsNullOrEmpty(status)
                ? contracts
                : contracts.Where(c => NormalizeStatus(c.Status) == status).ToList();

            var grouped = filteredContracts
                .GroupBy(c => NormalizeStatus(c.Status))
                .Select(g => new ContractStatistics
                {
                    Status = GetDisplayName(g.Key),
                    Count = g.Count()
                })
                .ToList();

            return grouped;
        }

        public async Task<List<TaskStatistics>> GetTaskStatisticsAsync(string status = null)
        {
            var tasks = await _apiService.GetAllTasksAsync();
            var filteredTasks = string.IsNullOrEmpty(status)
                ? tasks
                : tasks.Where(t => NormalizeStatus(t.Status) == status).ToList();

            var grouped = filteredTasks
                .GroupBy(t => NormalizeStatus(t.Status))
                .Select(g => new TaskStatistics
                {
                    Status = GetDisplayName(g.Key),
                    Count = g.Count()
                })
                .ToList();

            return grouped;
        }

        public async Task<List<StatusViewModel>> GetAllStatusesAsync()
        {
            try
            {
                var contracts = await _apiService.GetAllContractsAsync();
                var tasks = await _apiService.GetAllTasksAsync();

                var statuses = contracts.Select(c => NormalizeStatus(c.Status))
                    .Union(tasks.Select(t => NormalizeStatus(t.Status)))
                    .Distinct()
                    .Select(s => new StatusViewModel
                    {
                        Value = s,
                        DisplayName = GetDisplayName(s)
                    })
                    .ToList();

                return statuses.Any() ? statuses : DefaultStatuses.Select(s => new StatusViewModel
                {
                    Value = s,
                    DisplayName = GetDisplayName(s)
                }).ToList();
            }
            catch
            {
                return DefaultStatuses.Select(s => new StatusViewModel
                {
                    Value = s,
                    DisplayName = GetDisplayName(s)
                }).ToList();
            }
        }

        private string NormalizeStatus(object status)
        {
            if (status is int intStatus)
            {
                return intStatus.ToString();
            }
            return status?.ToString() ?? string.Empty;
        }

        private string GetDisplayName(string status)
        {
            return StatusDisplayNames.TryGetValue(status, out var displayName) ? displayName : status;
        }
    }
}