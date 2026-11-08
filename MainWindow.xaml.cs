using Microsoft.CodeAnalysis.Scripting;
using System.Net.Http;
using System.Security.Policy;
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

namespace GoldenLight
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            string url = UrlBox.Text;
            string code = "";
            if (MessageBox.Show($"You are running a script from {url}. Goldenlight has not verified this script.\n" +
                $"So, only continue if you trust the source.\n\n" +
                $"We cannot confirm the software" +
                $" has any protection or warranty.\n\n" +
                $"Click OK to continue, or Cancel to cancel.\n" , "Confirmation",
                MessageBoxButton.OKCancel, MessageBoxImage.Stop,
                MessageBoxResult.Cancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            try
            {
                using var http = new HttpClient();

                code =
                    await http.GetStringAsync(url);
            }
            catch (Exception ex) { MessageBox.Show($"Error! Cannot fetch the script. {ex}"); }
            try
            {
                await Microsoft.CodeAnalysis.CSharp.Scripting.CSharpScript.Create(code, ScriptOptions.Default.WithReferences(typeof(object).Assembly, typeof(Enumerable).Assembly).WithImports(["System", "Microsoft", "System.Collections.Generic", "System.Linq", "System.Console", "System.IO"])).RunAsync();
            }
            catch (Exception ex) { MessageBox.Show($"Error! Cannot run the script. {ex}"); }
        }
    }
}