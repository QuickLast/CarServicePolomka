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
    /// Логика взаимодействия для CustomersPage.xaml
    /// </summary>
    public partial class CustomersPage : Page
    {
        private MainWindow _mainWindow;
        public static List<Client> clients = new List<Client>();
        public static List<Client> filterClients = new List<Client>();
        public CustomersPage(MainWindow mainWindow)
        {
            try
            {
                InitializeComponent();
                _mainWindow = mainWindow;

                clients = App.db.Client.ToList();
                CustomersLv.ItemsSource = clients;
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
                filterClients = clients;
                if (!string.IsNullOrWhiteSpace(SearchTb.Text))
                {
                    string searchText = SearchTb.Text.Trim().ToLower();
                    filterClients = filterClients.Where(x => x.FullName.Trim().ToLower().Contains(searchText)).ToList();
                }
                CustomersLv.ItemsSource = filterClients;
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

        private void AddCustomerBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddCustomerWindow addCustomerWindow = new AddCustomerWindow();
                addCustomerWindow.ShowDialog();
                clients = App.db.Client.ToList();
                CustomersLv.ItemsSource = clients;
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
                App.selectedClient = (Client)button.DataContext;

                EditCustomerWindow editCustomerWindow = new EditCustomerWindow();
                editCustomerWindow.ShowDialog();

                clients = App.db.Client.ToList();
                CustomersLv.ItemsSource = clients;
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
                Client client = (Client)button.DataContext;

                MessageBoxResult result = MessageBox.Show("Вы точно хотите удалить клиента?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    App.db.Client.Remove(client);
                    App.db.SaveChanges();
                    MessageBox.Show("Клиент успешно удален.");
                }
                else
                {
                    MessageBox.Show("Отмена удаления.");
                }

                clients = App.db.Client.ToList();
                CustomersLv.ItemsSource = clients;
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
