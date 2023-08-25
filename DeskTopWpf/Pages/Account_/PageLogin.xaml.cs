using DeskTopWpf.Pages;
using DesktopWpfLib.Account;
using DesktopWpfLib.Data;
using DesktopWpfLib.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace DeskTopWpf.Account_
{
    /// <summary>
    /// Логика взаимодействия для PageLogin.xaml
    /// </summary>
    public partial class PageLogin : Page
    {
        private IAccount _account;
        private readonly IRepository<ApplicationView> _repository;
        public PageLogin(IRepository<ApplicationView> repository, IAccount account)
        {
            InitializeComponent();
            _account = account;
            _repository = repository;
        }
        public string userName { get { return NameBox.Text; } }
        public string Password { get { return PasswordBox.Password; } }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            LoginModel loginModel = new LoginModel { Login = userName, Password = this.Password };
            string[] result = await _account.Login(loginModel);
            if (int.Parse(result[0]) == 0)
            {
                NavigationService.Navigate(new Unsuccess(result[1]));

            }
            else
                NavigationService.Navigate(new PageApplications(_repository, _account));
        }
    }
}
