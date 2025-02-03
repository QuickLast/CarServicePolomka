using CarServicePolomka.Database;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для AddCustomerWindow.xaml
    /// </summary>
    public partial class AddCustomerWindow : Window
    {
        public AddCustomerWindow()
        {
            try
            {
                InitializeComponent();

                GenderCb.ItemsSource = App.db.Gender.ToList();
                BirthdayDp.DisplayDateEnd = DateTime.Now.AddYears(-18);
                BirthdayDp.SelectedDate = DateTime.Now.AddYears(-18);
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

        private void AddPhotoBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog()
                {
                    Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
                };
                if (openFileDialog.ShowDialog().GetValueOrDefault())
                {
                    CustomerImg.Source = new BitmapImage(new Uri(openFileDialog.FileName));
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
                var fieldsToCheck = new[] { SurnameTb.Text, NameTb.Text, PatronymicTb.Text, EmailTb.Text, PhoneNumberTb.Text, };
                if (fieldsToCheck.Any(string.IsNullOrWhiteSpace) || BirthdayDp.SelectedDate == null || GenderCb.SelectedItem == null)
                {
                    MessageBox.Show("Заполните все поля.");
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

                    Client client = new Client()
                    {
                        FirstName = SurnameTb.Text,
                        LastName = NameTb.Text,
                        Patronymic = PatronymicTb.Text,
                        Birthday = BirthdayDp.SelectedDate,
                        RegistrationDate = DateTime.Now,
                        Email = EmailTb.Text,
                        Phone = PhoneNumberTb.Text,
                        GenderCode = (GenderCb.SelectedItem as Gender).Code,
                    };

                    if (CustomerImg.Source != null)
                    {
                        var bitmap = CustomerImg.Source as BitmapImage;
                        if (bitmap != null)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                BitmapEncoder encoder = new PngBitmapEncoder();
                                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                                encoder.Save(memoryStream);
                                client.PhotoBinary = memoryStream.ToArray();
                            }
                        }
                    }
                    else
                    {
                        string defaultImagePath = "pack://application:,,,/CarService;component/Assets/Images/nophoto.png";
                        BitmapImage defaultImage = new BitmapImage(new Uri(defaultImagePath, UriKind.RelativeOrAbsolute));
                        using (var memoryStream = new MemoryStream())
                        {
                            BitmapEncoder encoder = new PngBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create(defaultImage));
                            encoder.Save(memoryStream);
                            client.PhotoBinary = memoryStream.ToArray();
                        }
                    }

                    App.db.Client.Add(client);
                    App.db.SaveChanges();

                    MessageBox.Show("Клиент успешно добавлен.");
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
