using RealtyCRMClient.DTOs;
using RealtyCRMClient.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace RealtyCRMClient.ViewModels
{
    public class ContractEditViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private readonly int _contractId;

        public ContractEditViewModel(int contractId)
        {
            _apiService = new ApiService();
            _contractId = contractId;

            LoadDataAsync();

            SaveCommand = new RelayCommand(SaveContract);
        }

        #region Properties

        private ContractDto _contract;
        public ContractDto Contract
        {
            get => _contract;
            set
            {
                _contract = value;
                OnPropertyChanged();
            }
        }

        private ClientDto _selectedClient;
        public ClientDto SelectedClient
        {
            get => _selectedClient;
            set
            {
                _selectedClient = value;
                if (_contract != null && _selectedClient != null)
                    _contract.ClientId = _selectedClient.Id;
                OnPropertyChanged();
            }
        }

        private PersonalDto _selectedAgent;
        public PersonalDto SelectedAgent
        {
            get => _selectedAgent;
            set
            {
                _selectedAgent = value;
                if (_contract != null && _selectedAgent != null)
                    _contract.AgentId = _selectedAgent.Id;
                OnPropertyChanged();
            }
        }

        private CardObjectRieltyDto _selectedRealEstate;
        public CardObjectRieltyDto SelectedRealEstate
        {
            get => _selectedRealEstate;
            set
            {
                _selectedRealEstate = value;
                if (_contract != null && _selectedRealEstate != null)
                    _contract.CardObjectRieltyId = _selectedRealEstate.Id;
                OnPropertyChanged();
            }
        }

        public List<string> Statuses { get; } = new List<string>
        {
            "Действует", "Завершён", "Расторгнут", "На паузе"
        };

        public ObservableCollection<ClientDto> Clients { get; } = new();
        public ObservableCollection<PersonalDto> Agents { get; } = new();
        public ObservableCollection<CardObjectRieltyDto> RealEstateObjects { get; set; } = new();

        public DateTimeOffset? StartDate
        {
            get => Contract?.StartDate;
            set
            {
                if (Contract != null) Contract.StartDate = value ?? DateTimeOffset.Now;
                OnPropertyChanged();
            }
        }

        public DateTimeOffset? EndDate
        {
            get => Contract?.EndDate;
            set
            {
                if (Contract != null) Contract.EndDate = value;
                OnPropertyChanged();
            }
        }

        public string Status
        {
            get => Contract?.Status;
            set
            {
                if (Contract != null) Contract.Status = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; }

        #endregion

        #region Private Methods

        private async Task LoadDataAsync()
        {
            Contract = await _apiService.GetContractByIdAsync(_contractId);
            if (Contract == null) return;

            var clients = await _apiService.GetAllClientsAsync();
            foreach (var client in clients)
                Clients.Add(client);

            var agents = await _apiService.GetAllPersonalsAsync();
            foreach (var agent in agents)
                Agents.Add(agent);


            var cardListItems = await _apiService.GetAllCardsAsync();
            var mappedCards = cardListItems.Select(c => CardMapper.MapToCardObjectRieltyDto(c)).ToList();

            RealEstateObjects = new ObservableCollection<CardObjectRieltyDto>(mappedCards);

            SelectedClient = Clients.FirstOrDefault(c => c.Id == Contract.ClientId);
            SelectedAgent = Agents.FirstOrDefault(a => a.Id == Contract.AgentId);
            SelectedRealEstate = RealEstateObjects.FirstOrDefault(r => r.Id == Contract.CardObjectRieltyId);
        }

        private async Task SaveContract()
        {
            try
            {



                await _apiService.UpdateContractAsync(_contractId, Contract);
                MessageBox.Show("Договор успешно обновлён.");
                Application.Current.Windows.OfType<EditContractWindow>().FirstOrDefault()?.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения договора: {ex.Message}");
            }
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

