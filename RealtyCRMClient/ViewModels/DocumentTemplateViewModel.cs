using Microsoft.Win32;
using RealtyCRMClient.DTOs;
using RealtyCRMClient.Models;
using RealtyCRMClient.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RealtyCRMClient.ViewModels
{
    public class DocumentTemplateViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private readonly PdfService _pdfService;



        public DocumentTemplateViewModel()
        {
            SelectBuyerCommand = new RelayCommand(OnSelectBuyer);
            SelectSellerCommand = new RelayCommand(OnSelectSeller);
            _apiService = new ApiService();
            _pdfService = new PdfService();

            LoadDataAsync();

            GeneratePdfCommand = new RelayCommand(async () => await OnGeneratePdf(), CanGeneratePdf);

        }

        #region Properties




        private DocumentTemplateDto _selectedTemplate;
        public DocumentTemplateDto SelectedTemplate
        {
            get => _selectedTemplate;
            set
            {
                _selectedTemplate = value;
                OnPropertyChanged();
                ((RelayCommand)GeneratePdfCommand).RaiseCanExecuteChanged();
            }
        }

        private string _clientSearchQuery;
        public string ClientSearchQuery
        {
            get => _clientSearchQuery;
            set
            {
                _clientSearchQuery = value;
                OnPropertyChanged();
                OnClientSearchQueryChanged(); // Это async void, но допустимо для UI-событий
            }
        }
        private async void OnClientSearchQueryChanged()
        {
            FilteredClients.Clear();

            if (string.IsNullOrWhiteSpace(ClientSearchQuery))
            {
                var allClients = await _apiService.GetAllClientsAsync();
                foreach (var c in allClients)
                    FilteredClients.Add(c);
            }
            else
            {
                var allClients = await _apiService.GetAllClientsAsync();
                var filtered = allClients
                    .Where(c => c.Name?.Contains(ClientSearchQuery, StringComparison.OrdinalIgnoreCase) == true);

                foreach (var c in filtered)
                    FilteredClients.Add(c);
            }
        }

        private ClientDto _selectedBuyer;
        private ClientDto _selectedSeller;
        private ClientDto _selectedClientForRole;
        public ClientDto SelectedBuyer
        {
            get => _selectedBuyer;
            set
            {
                _selectedBuyer = value;
                OnPropertyChanged();
                ((RelayCommand)GeneratePdfCommand).RaiseCanExecuteChanged();
            }
        }

        public ClientDto SelectedSeller
        {
            get => _selectedSeller;
            set
            {
                _selectedSeller = value;
                OnPropertyChanged();
                ((RelayCommand)GeneratePdfCommand).RaiseCanExecuteChanged();
            }
        }
        public ClientDto SelectedClientForRole
        {
            get => _selectedClientForRole;
            set
            {
                _selectedClientForRole = value;
                OnPropertyChanged();
            }
        }

        public ICommand SelectBuyerCommand { get; }
        public ICommand SelectSellerCommand { get; }


        private bool _isMarriedSeller;
        private bool _isMarriedBuyer;
        private bool _notarizedConsentProvided;

        public bool IsMarriedSeller
        {
            get => _isMarriedSeller;
            set
            {
                _isMarriedSeller = value;
                OnPropertyChanged();
            }
        }

        public bool IsMarriedBuyer
        {
            get => _isMarriedBuyer;
            set
            {
                _isMarriedBuyer = value;
                OnPropertyChanged();
            }
        }

        public bool NotarizedConsentProvided
        {
            get => _notarizedConsentProvided;
            set
            {
                _notarizedConsentProvided = value;
                OnPropertyChanged();
            }
        }




        public ObservableCollection<DocumentTemplateDto> Templates { get; } = new();
        public ObservableCollection<ClientDto> FilteredClients { get; } = new();

        public ICommand GeneratePdfCommand { get; }

        #endregion

        #region Private Methods

        private async Task OnSelectBuyer()
        {
            if (SelectedClientForRole != null)
                SelectedBuyer = SelectedClientForRole;
        }

        private async Task OnSelectSeller()
        {
            if (SelectedClientForRole != null)
                SelectedSeller = SelectedClientForRole;
        }

        private async Task LoadDataAsync()
        {
            var templates = await _apiService.GetAllTemplatesAsync();
            foreach (var template in templates)
                Templates.Add(template);

            var clients = await _apiService.GetAllClientsAsync();
            foreach (var client in clients)
                FilteredClients.Add(client);
        }

        private bool CanGeneratePdf() =>
    SelectedTemplate != null &&
    SelectedBuyer != null &&
    SelectedSeller != null;

        private async Task OnGeneratePdf()
        {
            try
            {
                var card = await _apiService.GetCardByClientIdAsync(282);
                if (card == null)
                {
                    MessageBox.Show("Не удалось загрузить данные о недвижимости.");
                    return;
                }

                var fields = new DocumentFields
                {
                    SellerName = SelectedSeller?.Name ?? "не указан",
                    //SellerAddress = SelectedSeller?.Description ?? "не указан",
                    BuyerName = SelectedBuyer?.Name ?? "не указан",
                    //BuyerAddress = SelectedBuyer?.Description ?? "не указан",
                    Address = card.Address ?? "не указан",
                    TotalArea = card.TotalArea ?? "не указано",
                    PriceNumeric = card.Price ?? "0 ₽",
                    PriceInWords = NumberToWords.Convert(card.Price),
                    IsMarriedSeller = IsMarriedSeller,
                    IsMarriedBuyer = IsMarriedBuyer,
                    NotarizedConsentProvided = NotarizedConsentProvided,
                    BankCellLocation = "г. Самара, ул. Ленина, д. 1"
                };

                var filledContent = TemplateProcessor.ReplaceFields(SelectedTemplate.Content, fields);
                filledContent = filledContent.Replace("\r", "").Replace('\x00A0', ' '); // удалить неразрывные пробелы
                filledContent = filledContent.Replace("\u200B", "");
                //string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                var dialog = new SaveFileDialog
                {
                    Filter = "PDF файл (*.pdf)|*.pdf",
                    FileName = "Договор_купли-продажи.pdf"
                };

                if (dialog.ShowDialog() != true)
                    MessageBox.Show("Ошибка открытия файла");
                string desktopPath = dialog.FileName;
                string filePath = desktopPath; // Path.Combine(desktopPath, "Договор_купли-продажи.pdf");

                _pdfService.GeneratePdf(filledContent, filePath);

                MessageBox.Show("PDF успешно создан!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при генерации PDF: {ex.Message}");
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


    //#region Helper Classes

    //public class DocumentFields
    //{
    //    // Продавец
    //    public string SellerName { get; set; } = string.Empty;
    //    public string SellerAddress { get; set; } = string.Empty;

    //    // Покупатель
    //    public string BuyerName { get; set; } = string.Empty;
    //    public string BuyerAddress { get; set; } = string.Empty;

    //    // Объект недвижимости
    //    public string Address { get; set; } = string.Empty;
    //    public string TotalArea { get; set; } = string.Empty;
    //    public string PriceNumeric { get; set; } = string.Empty;
    //    public string PriceInWords { get; set; } = string.Empty;

    //    // Условия
    //    public bool IsMarriedSeller { get; set; }
    //    public bool IsMarriedBuyer { get; set; }
    //    public bool NotarizedConsentProvided { get; set; }

    //    // Дополнительно
    //    public string BankCellLocation { get; set; } = string.Empty;
    //}

    //public static class TemplateProcessor
    //{
    //    public static string ReplaceFields(string template, DocumentFields fields)
    //    {
    //        var result = template;

    //        // Основные поля
    //        result = result.Replace("гр. РФ __________________________________,", $"гр. РФ {fields.SellerName},")
    //                       .Replace("(фамилия, имя отчество)", fields.SellerName)
    //                       .Replace("(адрес регистрации)", fields.SellerAddress)
    //                       .Replace("гр. РФ __________________________________, ____________________ года рождения,", $"гр. РФ {fields.BuyerName},")
    //                       .Replace("(адрес регистрации) ", fields.BuyerAddress)
    //                       .Replace("{цена цифрами}", fields.PriceNumeric)
    //                       .Replace("{цена словами}", fields.PriceInWords)
    //                       .Replace("{адрес квартиры}", fields.Address)
    //                       .Replace("{общая площадь}", fields.TotalArea)
    //                       .Replace("{местонахождение банковской ячейки}", fields.BankCellLocation);

    //        // Условные блоки — выбор варианта
    //        if (fields.IsMarriedSeller && !fields.NotarizedConsentProvided)
    //        {
    //            result = RemoveSection(result,
    //                "4.1.9. Квартира не находится в общей совместной собственности супругов…",
    //                "4.1.9. Покупатель получил и представил нотариально удостоверенное согласие второго супруга…");
    //        }
    //        else if (!fields.IsMarriedSeller)
    //        {
    //            result = RemoveSection(result,
    //                "4.1.9. Покупатель получил и представил нотариально удостоверенное согласие второго супруга…",
    //                "4.1.9. Квартира не находится в общей совместной собственности супругов…");
    //        }

    //        if (fields.IsMarriedBuyer)
    //        {
    //            result = ReplaceSection(result,
    //                "4.2.2. На момент заключения настоящего Договора Покупатель в браке не состоит.",
    //                "4.2.2. Покупатель получил и представил нотариально удостоверенное согласие второго супруга на заключение настоящего Договора на установленных в нем условиях.");
    //        }

    //        return result;
    //    }

    //    private static string RemoveSection(string text, string startMarker, string endMarker)
    //    {
    //        int startIndex = text.IndexOf(startMarker);
    //        int endIndex = text.IndexOf(endMarker, startIndex) + endMarker.Length;

    //        if (startIndex >= 0 && endIndex > startIndex)
    //        {
    //            text = text.Remove(startIndex, endIndex - startIndex);
    //        }

    //        return text;
    //    }

    //    private static string ReplaceSection(string text, string oldText, string newText)
    //    {
    //        return text.Replace(oldText, newText);
    //    }
    //}

    //#endregion
}

