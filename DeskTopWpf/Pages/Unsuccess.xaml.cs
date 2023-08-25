using Newtonsoft.Json.Linq;
using System.Windows.Controls;

namespace DeskTopWpf.Pages
{
    /// <summary>
    /// Логика взаимодействия для Unsuccess.xaml
    /// </summary>
    public partial class Unsuccess : Page
    {
        public Unsuccess(string text)
        {
            InitializeComponent();

            try
            {
                JObject jo = JObject.Parse(text);
                string error = jo["errors"].ToString().Trim('{', '}');
                Alert.Text = error;
            }
            catch
            {
                Alert.Text = text;
            }
        }
    }
}
