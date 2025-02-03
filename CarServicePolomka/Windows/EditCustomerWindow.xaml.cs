using CarServicePolomka.Database;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CarService.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditCustomerWindow.xaml
    /// </summary>
    public partial class EditCustomerWindow : Window
    {
        public EditCustomerWindow()
        {
            try
            {
                InitializeComponent();
                Upload();

                GenderCb.ItemsSource = App.db.Gender.ToList();
                BirthdayDp.DisplayDateEnd = DateTime.Now.AddYears(-18);
                RegistrationDateDp.DisplayDateEnd = DateTime.Today;
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
                SurnameTb.Text = App.selectedClient.FirstName;
                NameTb.Text = App.selectedClient.LastName;
                PatronymicTb.Text = App.selectedClient.Patronymic;
                BirthdayDp.SelectedDate = App.selectedClient.Birthday;
                RegistrationDateDp.SelectedDate = App.selectedClient.RegistrationDate;
                EmailTb.Text = App.selectedClient.Email;
                PhoneNumberTb.Text = App.selectedClient.Phone;
                GenderCb.SelectedItem = App.selectedClient.Gender;
                CustomerImg.Source = ToImage(App.selectedClient.PhotoBinary);
            }
            catch
            {
                MessageBox.Show("Произошла ошибка.");
            }
        }

        public BitmapImage ToImage(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                return image;
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

        private void PhoneNumberTb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                if (!char.IsDigit(e.Text, 0) && e.Text != "+")
                {
                    e.Handled = true;
                }
            }
            catch
            {
                MessageBox.Show("Произошла ошибка.");
            }
        }

        private void EditPhotoBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog()
                {
                    Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
                };
                if (openFileDialog.ShowDialog().GetValueOrDefault())
                {
                    App.selectedClient.PhotoBinary = File.ReadAllBytes(openFileDialog.FileName);
                    CustomerImg.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                }
                else
                {
                    MessageBox.Show("Изменений не происходило.");
                    return;
                }

                App.db.Client.AddOrUpdate(App.selectedClient);
                App.db.SaveChanges();

                MessageBox.Show("Фотография клиента выбрана.");
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
                var fieldsToCheck = new[] { SurnameTb.Text, NameTb.Text, PatronymicTb.Text, EmailTb.Text, PhoneNumberTb.Text, };
                if (fieldsToCheck.Any(string.IsNullOrWhiteSpace) || BirthdayDp.SelectedDate == null || RegistrationDateDp.SelectedDate == null || GenderCb.SelectedItem == null)
                {
                    MessageBox.Show("Заполните все поля.");
                }
                else if (SurnameTb.Text == App.selectedClient.FirstName && NameTb.Text == App.selectedClient.LastName && PatronymicTb.Text == App.selectedClient.Patronymic && EmailTb.Text == App.selectedClient.Email && PhoneNumberTb.Text == App.selectedClient.Phone && BirthdayDp.SelectedDate == App.selectedClient.Birthday && RegistrationDateDp.SelectedDate == App.selectedClient.RegistrationDate && GenderCb.SelectedItem == App.selectedClient.Gender)
                {
                    MessageBox.Show("Изменений не происходило.");
                    return;
                }
                else
                {
                    if (BirthdayDp.SelectedDate > DateTime.Now.AddYears(-18))
                    {
                        MessageBox.Show("Клиенту должно быть не менее 18 лет.");
                        return;
                    }

                    if (PhoneNumberTb.Text.Length > 12)
                    {
                        MessageBox.Show("Номер телефона должен иметь не более 12 символов.");
                        return;
                    }

                    App.selectedClient.FirstName = SurnameTb.Text;
                    App.selectedClient.LastName = NameTb.Text;
                    App.selectedClient.Patronymic = PatronymicTb.Text;
                    App.selectedClient.Birthday = BirthdayDp.SelectedDate;
                    App.selectedClient.RegistrationDate = (DateTime)RegistrationDateDp.SelectedDate;
                    App.selectedClient.Email = EmailTb.Text;
                    App.selectedClient.Phone = PhoneNumberTb.Text;
                    App.selectedClient.GenderCode = (GenderCb.SelectedItem as Gender).Code;

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
