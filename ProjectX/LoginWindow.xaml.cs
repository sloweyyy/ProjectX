using Emgu.CV.LineDescriptor;
using iTextSharp.text.pdf.qrcode;
using Newtonsoft.Json;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectX
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow
    {
        string apiKey = "";
        private void ReadAPIKey()
        {
            string path = "APIKey.txt";
            if (File.Exists(path))
            {
                apiKey = File.ReadAllText(path);
                ApiKeyTextBox.Text = apiKey;
            }
            else File.Create(path);
        }


        public LoginWindow()
        {
            InitializeComponent();
            ReadAPIKey();
        }
        private void CheckApiKey_Click(object sender, RoutedEventArgs e)
        {

            bool isValid = CheckKey(ApiKeyTextBox.Text);
            if (isValid)
            {
                File.WriteAllText("APIKey.txt", ApiKeyTextBox.Text);
                MainWindow mainWindow = new MainWindow(ApiKeyTextBox.Text);
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("API Key không hợp lệ.");
                ApiKeyTextBox.Text = "Nhập API Key vào đây";
            }
        }

        private bool CheckKey(string key)
        {
            var client = new RestClient("https://api.zalo.ai/v1/tts/synthesize");
            var request = new RestRequest(Method.POST);
            request.AddHeader("apikey", key);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("input", "Kiểm tra API key"); // Thêm dữ liệu mẫu

            var response = client.Execute(request);
            if (response.IsSuccessful)
            {
                var jsonResponse = JsonConvert.DeserializeObject<dynamic>(response.Content);
                int errorCode = jsonResponse.error_code;

                if (errorCode == 0)
                {
                    return true; // Key hợp lệ nếu errorCode là 0
                }
            }
            return false; // Trả về false nếu có lỗi hoặc errorCode không phải là 0
        }


        private void GetApiKey_Click(object sender, RoutedEventArgs e)
        {
            File.Open("D:/Code Workspace/WPF/ProjectX/ProjectX/API.pdf", FileMode.Open);
        }
    }
}
