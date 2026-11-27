using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DnevnikNastroeniya.Data;
using DnevnikNastroeniya.Helpers;
using DnevnikNastroeniya.Models;
using DnevnikNastroeniya.Services;
using DnevnikNastroeniya.Windows;

namespace DnevnikNastroeniya.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly AppDbContext _context;

        private string _userName;
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        private string _lastRecordText;
        public string LastRecordText
        {
            get => _lastRecordText;
            set => SetProperty(ref _lastRecordText, value);
        }

        private ObservableCollection<Record> _records;
        public ObservableCollection<Record> Records
        {
            get => _records;
            set => SetProperty(ref _records, value);
        }

        private Forecast? _lastForecast;
        public Forecast? LastForecast
        {
            get => _lastForecast;
            set => SetProperty(ref _lastForecast, value);
        }

        private bool _isForecastLoading;
        public bool IsForecastLoading
        {
            get => _isForecastLoading;
            set => SetProperty(ref _isForecastLoading, value);
        }

        public ICommand AddRecordCommand { get; }
        public ICommand GenerateForecastCommand { get; }
        public ICommand LogoutCommand { get; }

        public MainWindowViewModel()
        {
            _context = new AppDbContext();
            UserName = AppState.CurrentUserName;
            Records = new ObservableCollection<Record>();

            AddRecordCommand = new RelayCommand(_ => AddRecord());
            GenerateForecastCommand = new RelayCommand(async _ => await GenerateForecast(), _ => !IsForecastLoading);
            LogoutCommand = new RelayCommand(_ => Logout());

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var lastRecord = _context.Records
                    .Where(r => r.IdClient == AppState.CurrentClientId)
                    .OrderByDescending(r => r.CreationDate)
                    .FirstOrDefault();

                if (lastRecord != null)
                {
                    LastRecordText = $"Настроение: {lastRecord.Mood_Level} | {lastRecord.Comment}";
                }
                else
                {
                    LastRecordText = "У вас пока нет записей.";
                }

                var allRecords = _context.Records
                    .Where(r => r.IdClient == AppState.CurrentClientId)
                    .OrderByDescending(r => r.CreationDate)
                    .ToList();

                Records.Clear();
                foreach (var rec in allRecords)
                {
                    Records.Add(rec);
                }

                LastForecast = _context.Forecasts
                    .Where(f => f.Note.IdClient == AppState.CurrentClientId)
                    .OrderByDescending(f => f.CreationDate)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки: " + ex.Message);
            }
        }

        private void AddRecord()
        {
            var addWindow = new AddRecordWindow();
            if (addWindow.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private async Task GenerateForecast()
        {
            IsForecastLoading = true;
            try
            {
                IForecastStrategy forecastService = new NeuralForecastService();
                LastForecast = await forecastService.GenerateForecastAsync(AppState.CurrentClientId);
                MessageBox.Show("Прогноз успешно сгенерирован!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Ошибка API DeepSeek. Проверьте баланс и интернет.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsForecastLoading = false;
            }
        }

        private void Logout()
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            Application.Current.Windows[0]?.Close();
        }
    }
}