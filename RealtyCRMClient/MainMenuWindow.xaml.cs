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

        // Открыть Задачи (заглушка)
        private void OpenTasks_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Раздел 'Задачи' находится в разработке");
        }

        // Открыть Документы (заглушка)
        private void OpenDocuments_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Раздел 'Документы' находится в разработке");
        }
        private void ExitApplication_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); // Завершить приложение
        }
    }
}