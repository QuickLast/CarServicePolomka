using CarServicePolomka.Database;
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

namespace CarService.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddVisitWindow.xaml
    /// </summary>
    public partial class AddVisitWindow : Window
    {
        public AddVisitWindow()
        {
            try
            {
                InitializeComponent();

                CustomerCb.ItemsSource = App.db.Client.ToList();
                ServiceCb.ItemsSource = App.db.Service.ToList();
            }
            catch
            {
                MessageBox.Show("Произошла ошибка.");
            }
        }

        private void ComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (sender is ComboBox comboBox)
                {
                    if (!string.IsNullOrWhiteSpace(comboBox.Text))
                    {
                        comboBox.IsDropDownOpen = true;
                    }
                    else
                    {
                        comboBox.IsDropDownOpen = false;
                    }

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
                if (CustomerCb.SelectedItem == null || ServiceCb.SelectedItem == null || string.IsNullOrWhiteSpace(CommentTb.Text))
                {
                    MessageBox.Show("Заполните все поля.");
                }
                else
                {
                    ClientService clientService = new ClientService()
                    {
                        ClientID = (CustomerCb.SelectedItem as Client).ID,
                        ServiceID = (ServiceCb.SelectedItem as Service).ID,
                        StartTime = DateTime.Now,
                        Comment = CommentTb.Text,
                    };

                    App.db.ClientService.Add(clientService);
                    App.db.SaveChanges();

                    MessageBox.Show("Посещение успешно добавлено.");
                    Close();
                }
            }
            catch
            {
                MessageBox.Show("Произошла ошибка.");
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch
            {
                MessageBox.Show("Произошла ошибка.");
            }
        }
    }
}
