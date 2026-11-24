using DnevnikNastroeniya.Data;
using DnevnikNastroeniya.Helpers;
using DnevnikNastroeniya.Interfaces;
using DnevnikNastroeniya.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DnevnikNastroeniya.Windows
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window,ITransitionWindow
    {
        private AppDbContext _context;
        public RegistrationWindow()
        {
            InitializeComponent();
            _context = new AppDbContext();
        }
        public void ToWindow()
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ToWindow();
        }


        private void EnterText_MouseEnter(object sender, MouseEventArgs e)
        {
            EnterText.FontWeight = FontWeights.ExtraBold;
        }
        private void EnterText_MouseLeave(object sender, MouseEventArgs e)
        {
            EnterText.FontWeight = FontWeights.Bold;
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            string login = TxtLogin.Text.Trim();
            string pass = TxtPassword.Password;
            string pass2 = TxtPassword2.Password;
            string fio = TxtFOI.Text.Trim();

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(pass) || string.IsNullOrWhiteSpace(fio) || string.IsNullOrWhiteSpace(pass2))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (pass != pass2)
            {
                MessageBox.Show("Пароли не совпадают!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                bool userExists = _context.Clients.Any(c => c.Login == login);
                if (userExists)
                {
                    MessageBox.Show("Пользователь с таким логином уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var newClient = new Client
                {
                    Login = login,
                    Password = PasswordHelper.HashPassword(pass),
                    FIO = fio,
                    DateOfCreate = DateTime.Now
                };

                _context.Clients.Add(newClient);
                _context.SaveChanges();

                MessageBox.Show("Регистрация успешна", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                LoginWindow loginWin = new LoginWindow();
                loginWin.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка Базы Данных" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
