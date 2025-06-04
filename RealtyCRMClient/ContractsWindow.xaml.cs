using RealtyCRMClient.ViewModels;
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

namespace RealtyCRMClient
{
    /// <summary>
    /// Логика взаимодействия для ContractsWindow.xaml
    /// </summary>
    public partial class ContractsWindow : Window
    {
        public ContractsWindow()
        {
            InitializeComponent();
            DataContext = new ContractsViewModel();
        }
        private void OpenEditWindow(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is ContractsViewModel vm && vm.SelectedContract != null)
            {
                var window = new EditContractWindow(vm.SelectedContract);
                window.Closed += (s, args) => vm.LoadContractsAsync(); // Перезагрузка данных после закрытия
                window.Show();
            }
        }
    }
}
