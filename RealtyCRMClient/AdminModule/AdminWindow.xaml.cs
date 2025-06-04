using RealtyCRMClient.AdminModule.Services;
using RealtyCRMClient.AdminModule.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Serilog;

namespace RealtyCRMClient.AdminModule
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private readonly ILogger _logger;

        public AdminWindow(ApiService apiService, ILogger logger)
        {
            InitializeComponent();
            _logger = logger;
            DataContext = new AdminWindowViewModel(new EntityService(apiService, logger), logger);
        }

        /// <summary>
        /// Валидация ввода только чисел в TextBox.
        /// </summary>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !long.TryParse(e.Text, out _);
            if (e.Handled)
            {
                _logger.Information("Некорректный ввод в TextBox: {Input}", e.Text);
            }
        }

        /// <summary>
        /// Временный обработчик для диагностики нажатия кнопки.
        /// </summary>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            _logger.Information("Кнопка 'Удалить' нажата");
        }
    }

    /// <summary>
    /// Конвертер для преобразования текста в long.
    /// </summary>
    public class LongConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value?.ToString() ?? "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string str && long.TryParse(str, out long result))
            {
                return result;
            }
            return 0L; // Возвращаем 0, если ввод некорректен
        }
    }
}
