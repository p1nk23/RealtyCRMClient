using RealtyCRMClient.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RealtyCRMClient.ViewModels
{
    public class ContractsViewModel : INotifyPropertyChanged
    {

        private ICommand _editContractCommand;
        public ICommand EditContractCommand => _editContractCommand ??= new RelayCommand(OnEditContract, CanEditContract);

        private bool CanEditContract() => SelectedContract != null;

        private async Task OnEditContract()
        {
            var editWindow = new EditContractWindow(SelectedContract);
            editWindow.ShowDialog();
        }


        private readonly ApiService _apiService;

        public ContractsViewModel()
        {
            _apiService = new ApiService();
            LoadContractsAsync();
        }

        #region Properties

        private ObservableCollection<ContractDto> _contracts = new();
        public ObservableCollection<ContractDto> Contracts
        {
            get => _contracts;
            set
            {
                _contracts = value;
                OnPropertyChanged();
            }
        }

        private ContractDto _selectedContract;
        public ContractDto SelectedContract
        {
            get => _selectedContract;
            set
            {
                _selectedContract = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedContractDetails));
            }
        }

        public string SelectedContractDetails => SelectedContract == null ? "" :
            $"Клиент: {SelectedContract.Client?.Name ?? "—"}\n" +
            $"Агент: {SelectedContract.Agent?.Name ?? "—"}\n" +
            $"Объект: {SelectedContract.CardObjectRielty?.Address ?? "—"}\n" +
            $"Цена: {SelectedContract.CardObjectRielty?.Price ?? "—"}\n" +
            $"Дата начала: {SelectedContract.StartDate:dd.MM.yyyy}\n" +
            $"Статус: {SelectedContract.Status}";

        #endregion

        #region Private Methods

        public async Task LoadContractsAsync()
        {
            var contracts = await _apiService.GetAllContractsAsync();
            foreach (var contract in contracts)
                Contracts.Add(contract);
        }

        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
