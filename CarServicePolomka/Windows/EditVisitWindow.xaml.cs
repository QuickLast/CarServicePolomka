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
    /// Логика взаимодействия для EditVisitWindow.xaml
    /// </summary>
    public partial class EditVisitWindow : Window
    {
        public EditVisitWindow()
        {
            try
            {
                InitializeComponent();


                CustomerCb.ItemsSource = App.db.Client.ToList();
                ServiceCb.ItemsSource = App.db.Service.ToList();
                StartTimeDp.DisplayDateEnd = DateTime.Today;
                Upload();
            }
            catch
            {
                MessageBox.Show("Произошла ошибка.");
            }
        }

        private void Upload()
        {
            try
            {
                CustomerCb.SelectedItem = App.selectedVisit.Client;
                CustomerCb.IsDropDownOpen = false;
                ServiceCb.SelectedItem = App.selectedVisit.Service;
                ServiceCb.IsDropDownOpen = false;
                StartTimeDp.SelectedDate = App.selectedVisit.StartTime;
                CommentTb.Text = App.selectedVisit.Comment;
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

        private void ChangeBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CustomerCb.SelectedItem == null || ServiceCb.SelectedItem == null || StartTimeDp.SelectedDate == null || string.IsNullOrWhiteSpace(CommentTb.Text))
                {
                    MessageBox.Show("Заполните все поля.");
                }
                else if (CustomerCb.SelectedItem == App.selectedVisit.Client && ServiceCb.SelectedItem == App.selectedVisit.Service && StartTimeDp.SelectedDate == App.selectedVisit.StartTime && CommentTb.Text == App.selectedVisit.Comment)
                {
                    MessageBox.Show("Изменений не происходило.");
                    return;
                }
                else
                {
                    App.selectedVisit.ClientID = (CustomerCb.SelectedItem as Client).ID;
                    App.selectedVisit.ServiceID = (ServiceCb.SelectedItem as Service).ID;
                    App.selectedVisit.StartTime = (DateTime)StartTimeDp.SelectedDate;
                    App.selectedVisit.Comment = CommentTb.Text;

                    App.db.SaveChanges();

                    MessageBox.Show("Данные изменены.");
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
