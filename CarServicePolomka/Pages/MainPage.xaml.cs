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
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private MainWindow _mainWindow;
        public MainPage(MainWindow mainWindow)
        {
            try
            {
                InitializeComponent();
                _mainWindow = mainWindow;
            }
            catch
            {
                MessageBox.Show("Произошла ошибка.");
            }
        }

        private void CustomersBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new CustomersPage(_mainWindow));
            }
            catch
            {
                MessageBox.Show("Произошла ошибка.");
            }
        }

        private void VisitsBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new VisitsPage(_mainWindow));
            }
            catch
            {
                MessageBox.Show("Произошла ошибка.");
            }
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _mainWindow.Close();
            }
            catch
            {
                MessageBox.Show("Произошла ошибка.");
            }
        }
    }
}
