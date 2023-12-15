using RestSharp;
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

namespace ProjectX.Views
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        private string currentVersion = File.ReadAllText("..\\..\\version.txt");

        public About()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void WebsiteLink_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://projectx-landing-page.vercel.app/");
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var client = new RestClient("https://raw.githubusercontent.com/sloweyyy/IT008.O12/main/ProjectX/version.txt");

            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            if (!response.Content.Contains(currentVersion))
            {
                MessageBox.Show("Đã có phiên bản mới. Hãy cập nhật nhé!");
                System.Diagnostics.Process.Start("https://github.com/sloweyyy/IT008.O12/releases");
            }
            else
            {
                MessageBox.Show("Bạn đang sử dụng phiên bản mới nhất.");
            }
        }

        private void EmailLink_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("mailto: sloweycontact@gmail.com");
        }


    }
}
