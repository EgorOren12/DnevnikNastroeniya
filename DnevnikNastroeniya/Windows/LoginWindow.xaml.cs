using DnevnikNastroeniya.Data;
using DnevnikNastroeniya.Helpers;
using DnevnikNastroeniya.Interfaces;
using DnevnikNastroeniya.Windows;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DnevnikNastroeniya
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window, ITransitionWindow
    {
        private AppDbContext _context;
        public LoginWindow()
        {
            InitializeComponent();
            _context = new AppDbContext();
            _context.Database.EnsureCreated();
        }

        public void ToWindow()
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
            this.Close();
        }

        private void RegistrationTextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ToWindow();
        }

        private void RegistrationTextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            RegistrationTextBlock.FontWeight = FontWeights.ExtraBold;
        }

        private void RegistrationTextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            RegistrationTextBlock.FontWeight = FontWeights.Bold;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = TxtLogin.Text.Trim();
            string pass = TxtPassword.Password;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("Введите логин и пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var client = _context.Clients.FirstOrDefault(c => c.Login == login);

                if (client == null)
                {
                    MessageBox.Show("Пользователь не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string enteredHash = PasswordHelper.HashPassword(pass);
                if (client.Password == enteredHash)
                {
                    MessageBox.Show("Успешный вход!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainWindow mainwindow = new MainWindow();
                    mainwindow.Show();

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверный пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    TxtPassword.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка Базы ДанныхД: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    


}