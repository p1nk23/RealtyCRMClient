using RealtyCRMClient.Models;
using RealtyCRMClient.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Linq;
using System;
using System.Threading;

namespace RealtyCRMClient.ViewModels
{
    public class AnalyticsViewModel : INotifyPropertyChanged
    {
        private readonly StatisticsService _statisticsService;
        private ObservableCollection<object> _statisticsData;
        private ObservableCollection<StatusViewModel> _statuses;
        private StatusViewModel _selectedStatus;
        private bool _isLoading;
        private string _errorMessage;
        private ISeries[] _chartSeries;
        private Axis[] _xAxes;
        private Axis[] _yAxes;
        private CancellationTokenSource _cts;

        public AnalyticsViewModel(StatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
            _statisticsData = new ObservableCollection<object>();
            _statuses = new ObservableCollection<StatusViewModel>();
            RefreshCommand = new RelayCommand(async () => await LoadStatisticsAsync());
            _cts = new CancellationTokenSource();
            LoadInitialDataAsync();
        }

        public ObservableCollection<object> StatisticsData
        {
            get => _statisticsData;
            set
            {
                _statisticsData = value;
                OnPropertyChanged(nameof(StatisticsData));
            }
        }

        public ObservableCollection<StatusViewModel> Statuses
        {
            get => _statuses;
            set
            {
                _statuses = value;
                OnPropertyChanged(nameof(Statuses));
            }
        }

        public StatusViewModel SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                _selectedStatus = value;
                OnPropertyChanged(nameof(SelectedStatus));
                Task.Run(() => LoadStatisticsAsync());
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public ISeries[] ChartSeries
        {
            get => _chartSeries;
            set
            {
                _chartSeries = value;
                OnPropertyChanged(nameof(ChartSeries));
            }
        }

        public Axis[] XAxes
        {
            get => _xAxes;
            set
            {
                _xAxes = value;
                OnPropertyChanged(nameof(XAxes));
            }
        }

        public Axis[] YAxes
        {
            get => _yAxes;
            set
            {
                _yAxes = value;
                OnPropertyChanged(nameof(YAxes));
            }
        }

        public ICommand RefreshCommand { get; }

        private async void LoadInitialDataAsync()
        {
            try
            {
                IsLoading = true;
                var statuses = await _statisticsService.GetAllStatusesAsync();
                Statuses = new ObservableCollection<StatusViewModel>(statuses);
                await LoadStatisticsAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка загрузки данных: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadStatisticsAsync()
        {
            if (IsLoading)
            {
                _cts.Cancel();
                _cts = new CancellationTokenSource();
            }

            try
            {
                IsLoading = true;
                ErrorMessage = null;

                var statusValue = SelectedStatus?.Value;
                var contractStats = await _statisticsService.GetContractStatisticsAsync(statusValue);
                var taskStats = await _statisticsService.GetTaskStatisticsAsync(statusValue);

                StatisticsData.Clear();
                foreach (var stat in contractStats)
                    StatisticsData.Add(stat);
                foreach (var stat in taskStats)
                    StatisticsData.Add(stat);

                var statuses = StatisticsData.Select(s => s is ContractStatistics cs ? cs.Status : (s as TaskStatistics).Status).Distinct().ToArray();
                var contractCounts = contractStats.ToDictionary(s => s.Status, s => s.Count);
                var taskCounts = taskStats.ToDictionary(s => s.Status, s => s.Count);

                ChartSeries = new ISeries[]
                {
                    new ColumnSeries<int>
                    {
                        Name = "Договоры",
                        Values = statuses.Select(s => contractCounts.ContainsKey(s) ? contractCounts[s] : 0).ToArray(),
                        Fill = new SolidColorPaint(SKColors.Blue)
                    },
                    new ColumnSeries<int>
                    {
                        Name = "Задачи",
                        Values = statuses.Select(s => taskCounts.ContainsKey(s) ? taskCounts[s] : 0).ToArray(),
                        Fill = new SolidColorPaint(SKColors.Green)
                    }
                };

                XAxes = new[] { new Axis { Labels = statuses } };
                YAxes = new[] { new Axis { Name = "Количество", MinStep = 1 } };
            }
            catch (Exception ex)
            {
                if (ex is OperationCanceledException)
                    return;
                ErrorMessage = $"Ошибка загрузки статистики: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}