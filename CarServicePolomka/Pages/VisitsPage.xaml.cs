using CarServicePolomka.Database;
using CarService.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CarService.Pages
{
    /// <summary>
    /// Логика взаимодействия для VisitsPage.xaml
    /// </summary>
    public partial class VisitsPage : Page
    {
        private MainWindow _mainWindow;
        public static List<ClientService> clientServices = new List<ClientService>();
        public static List<ClientService> filterClientServices = new List<ClientService>();
        public VisitsPage(MainWindow mainWindow)
        {
            try
            {
                InitializeComponent();
                _mainWindow = mainWindow;

                clientServices = App.db.ClientService.ToList();
                VisitsLv.ItemsSource = clientServices;
            }
            catch
            {
                MessageBox.Show("Произошла ошибка.");
            }
        }

        private void Refresh()
        {
            try
            {
                filterClientServices = clientServices;
                if (!string.IsNullOrWhiteSpace(SearchTb.Text))
                {
                    string searchText = SearchTb.Text.Trim().ToLower();
                    filterClientServices = filterClientServices.Where(x => x.Client.FullName.Trim().ToLower().Contains(searchText) || x.Service.Title.Trim().ToLower().Contains(searchText)).ToList();
                }
                VisitsLv.ItemsSource = filterClientServices;
            }
            catch
            {
                MessageBox.Show("Произошла ошибка.");
            }
        }

        private void SearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Refresh();
            }
            catch
            {
                MessageBox.Show("Произошла ошибка.");
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                if (!char.IsLetter(e.Text, 0))
                {
                    e.Handled = true;
                }
            }
            catch
            {
                MessageBox.Show("Произошла ошибка.");
            }
        }

        private void AddVisitBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddVisitWindow addVisitWindow = new AddVisitWindow();
                addVisitWindow.ShowDialog();
                clientServices = App.db.ClientService.ToList();
                VisitsLv.ItemsSource = clientServices;
            }
            catch
            {
                MessageBox.Show("Произошла ошибка.");
            }
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = (Button)sender;
                App.selectedVisit = (ClientService)button.DataContext;

                EditVisitWindow editVisitWindow = new EditVisitWindow();
                editVisitWindow.ShowDialog();

                clientServices = App.db.ClientService.ToList();
                VisitsLv.ItemsSource = clientServices;
            }
            catch
            {
                MessageBox.Show("Произошла ошибка.");
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = (Button)sender;
                ClientService clientService = (ClientService)button.DataContext;

                MessageBoxResult result = MessageBox.Show("Вы точно хотите удалить посещение?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    App.db.ClientService.Remove(clientService);
                    App.db.SaveChanges();
                    MessageBox.Show("Посещение успешно удалено.");
                }
                else
                {
                    MessageBox.Show("Отмена удаления.");
                }

                clientServices = App.db.ClientService.ToList();
                VisitsLv.ItemsSource = clientServices;
            }
            catch
            {
                MessageBox.Show("Произошла ошибка.");
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new MainPage(_mainWindow));
            }
            catch
            {
                MessageBox.Show("Произошла ошибка.");
            }
        }
    }
}
