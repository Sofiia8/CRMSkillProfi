using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using DesktopWpfLib.Models;
using DesktopWpfLib.Data;
using DesktopWpfLib.Account;
using FontAwesome.WPF;
using System.Windows.Media;
using DeskTopWpf.Account_;

namespace DeskTopWpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IRepository<ApplicationView> _repositoryApplications;
        private readonly IRepositorySettings _repositorySettings;
        private readonly IAccount _account;
        private static Dictionary<int, string> applicationStatus = new Dictionary<int, string>()
        {
            { 1, "Получена" },
            { 2, "В работе" },
            { 3, "Выполнена" },
            { 4, "Отклонена" },
            { 5, "Отменена" }
        };
        private readonly MenuPanelModel _menuPanelModel;

        public MainWindow()
        {
            InitializeComponent();
            _repositoryApplications = new RepositoryApi();
            _repositorySettings = new RepositorySettingsApi();
            _account = new Account();

            _menuPanelModel = new MenuPanelModel();
            this.DataContext = _menuPanelModel;

        }
        
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            _menuPanelModel.ButtonFlag = (_menuPanelModel.ButtonFlag == 0) ? 1 : 0;
            BindingOperations.GetBindingExpression(menuPanel, UIElement.VisibilityProperty)?.UpdateTarget();
        }

        private void AddPresetButton_Click(object sender, RoutedEventArgs e)
        {
            var addButton = sender as FrameworkElement;
            if (addButton != null)
            {
                addButton.ContextMenu.IsOpen = true;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new PageLogin(_repositoryApplications, _account));   
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            _account.Logout();
            _mainFrame.Navigate(new PageLogin(_repositoryApplications, _account));
        }

        private void ButtonDeskTop_Click(object sender, RoutedEventArgs e)
        {
            if (!_account.Authorized)
                _mainFrame.Navigate(new PageLogin(_repositoryApplications, _account));
            else
                _mainFrame.Navigate(new PageApplications(_repositoryApplications, _account));
        }
    }
}
