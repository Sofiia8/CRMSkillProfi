using DesktopWpfLib.Data;
using DesktopWpfLib.Models;
using System.Threading.Tasks;
using System;
using System.Windows;
using System.Windows.Controls;
using DesktopWpfLib.Account;
using System.Linq;
using System.ComponentModel;
using System.Net;
using System.Collections.Generic;
using DeskTopWpf.Pages;

namespace DeskTopWpf
{
    /// <summary>
    /// Логика взаимодействия для PageApplications.xaml
    /// </summary>
    public partial class PageApplications : Page, INotifyPropertyChanged
    {
        private IRepository<ApplicationView> _repositoryApplications;
        private IAccount _account;
        private DateTime _dateFrom;
        private DateTime _dateTo;
        private static Dictionary<int, string> applicationStatus = new Dictionary<int, string>()
        {
            { 1, "Получена" },
            { 2, "В работе" },
            { 3, "Выполнена" },
            { 4, "Отклонена" },
            { 5, "Отменена" }
        };
        public DateTime DateFrom
        {
            get { return _dateFrom; }
            set
            {
                if (_dateFrom != value)
                {
                    _dateFrom = value;
                    OnPropertyChanged(nameof(DateFrom));
                }
            }
        }
        public DateTime DateTo
        {
            get { return _dateTo; }
            set
            {
                if (_dateTo != value)
                {
                    _dateTo = value;
                    OnPropertyChanged(nameof(DateTo));
                }
            }
        }

        public int CountAll { get; set; }
        public int CountPeriod { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public PageApplications(IRepository<ApplicationView> repository, IAccount account)
        {
            InitializeComponent();
            DataContext = this;
            _repositoryApplications = repository;
            _account = account;
            DateFrom = DateTime.Today.AddDays(-1);
            DateTo = DateTime.Today;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await GetItemsFromToDates();
        }

        private async void Button_ClickAsync(object sender, RoutedEventArgs e)
        {
            await GetItemsFromToDates();
        }

        private async Task GetItemsFromToDates()
        {
            try
            {
                var jwt = _account.Token;
                var allItems = await _repositoryApplications.GetItems(jwt);
                if (allItems == null)
                {
                    NavigationService.Navigate(new Unsuccess("Сервис не доступен"));
                    return; 
                }
                CountAll = allItems.Count();
                countAll.Text = "Всего заявок: " + CountAll;
                var answer = await _repositoryApplications.GetItemsFromToDates(DateFrom, DateTo, jwt);
                if (answer != null)
                {
                    CountPeriod = answer.Count();
                    countPeriod.Text = "За данный период поступило заявок: " + CountPeriod;
                    personList.ItemsSource = answer;
                }
                else
                {
                    NavigationService.Navigate(new Unsuccess("Сервис не доступен"));
                    return;
                }
            }
            catch (Exception ex) {
                NavigationService.Navigate(new Unsuccess("Ошибка"));
            }
        }

        private async void Button_Click_Yesterday(object sender, RoutedEventArgs e)
        {
            DateFrom = DateTime.Today.AddDays(-1);
            DateTo = DateTime.Today.AddDays(-1);
            await GetItemsFromToDates();
        }

        private async void Button_Click_Today(object sender, RoutedEventArgs e)
        {
            DateFrom = DateTime.Today;
            DateTo = DateTime.Today;
            await GetItemsFromToDates();
        }

        private async void Button_Click_Week(object sender, RoutedEventArgs e)
        {
            DateFrom = DateTime.Today.AddDays(Convert.ToInt32(DateTime.Now.DayOfWeek) == 0 ? -6 :
                1 - Convert.ToInt32(DateTime.Now.DayOfWeek));
            DateTo = DateTime.Today;
            await GetItemsFromToDates();
        }

        private async void Button_Click_Month(object sender, RoutedEventArgs e)
        {
            DateFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            await GetItemsFromToDates();
        }

        private void ChooseStatusButton_Click(object sender, RoutedEventArgs e)
        {
            var addButton = sender as FrameworkElement;
            if (addButton != null)
            {
                addButton.ContextMenu.IsOpen = true;
            }
        }
        private async void NewStReceived_Click(object sender, RoutedEventArgs e)
        {
            int applicationId = (personList.SelectedItem as ApplicationView).Id;
            await EditStatus(applicationId, applicationStatus[1]);
            (personList.SelectedItem as ApplicationView).Status = applicationStatus[2];
        }
        private async void NewStInWork_Click(object sender, RoutedEventArgs e)
        {
            int applicationId = (personList.SelectedItem as ApplicationView).Id;
            await EditStatus(applicationId, applicationStatus[2]);
            (personList.SelectedItem as ApplicationView).Status = applicationStatus[2];
        }

        private async void NewStDone_Click(object sender, RoutedEventArgs e)
        {
            int applicationId = (personList.SelectedItem as ApplicationView).Id;
            await EditStatus(applicationId, applicationStatus[3]);
            (personList.SelectedItem as ApplicationView).Status = applicationStatus[3];
        }

        private async void NewStRejected_Click(object sender, RoutedEventArgs e)
        {
            int applicationId = (personList.SelectedItem as ApplicationView).Id;
            await EditStatus(applicationId, applicationStatus[4]);
            (personList.SelectedItem as ApplicationView).Status = applicationStatus[4];
        }

        private async void NewStCancelled_Click(object sender, RoutedEventArgs e)
        {
            int applicationId = (personList.SelectedItem as ApplicationView).Id;
            await EditStatus(applicationId, applicationStatus[5]);
            (personList.SelectedItem as ApplicationView).Status = applicationStatus[5];
        }
        public async Task EditStatus(int id, string status)
        {
            var jwt = _account.Token;
            if (jwt == null) return;

            var result = await _repositoryApplications.EditRecord(id, status, jwt);
            if (result != HttpStatusCode.OK)
            {
                NavigationService.Navigate(new Unsuccess("Не удалось изменить запись, ошибка"));
            }
        }
    }
}
