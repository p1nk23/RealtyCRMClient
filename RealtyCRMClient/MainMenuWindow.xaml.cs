using RealtyCRMClient.DTOs;
using System.Windows;

namespace RealtyCRMClient
{
    public partial class MainMenuWindow : Window
    {
        public PersonalDto CurrentUser { get; }


        public MainMenuWindow(PersonalDto user)
        {
            InitializeComponent();
            CurrentUser = user;
            DataContext = this;
        }

        //Договоры
        private void OpenContracts_Click(object sender, RoutedEventArgs e)
        {
            var window = new ContractsWindow();
            window.Show();
        }

        //Аналитика
        private void OpenAnalytics_Click(object sender, RoutedEventArgs e)
        {
            var analyticsWindow = new AnalyticsWindow();
            analyticsWindow.Show(); // Или ShowDialog() для модального окна
        }


        // Сменить пользователя
        private void SwitchUser_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        // Открыть базу объектов (MainWindow)
        private void OpenObjects_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow(CurrentUser);
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
            this.Close();
        }

        private void OpenClients_Click(object sender, RoutedEventArgs e)
        {
            var clientsWindow = new ClientsWindow(CurrentUser); // Передаем пользователя
            clientsWindow.Show();
            this.Close();
        }

        // Открыть Задачи
        private void OpenTasks_Click(object sender, RoutedEventArgs e)
        {
            var tasksWindow = new TasksWindow(CurrentUser);
            tasksWindow.Show();
            this.Close();
        }

        // Открыть Документы
        private void OpenDocuments_Click(object sender, RoutedEventArgs e)
        {
            var docsWindow = new DocumentsTemplateWindow();
            docsWindow.Show();
        }
        private void ExitApplication_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); // Завершить приложение
        }
    }
}