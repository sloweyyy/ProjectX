using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using BCrypt.Net;
using RestSharp;
using System.Windows.Controls;
using System.Windows;
using System.Diagnostics;
using System.Windows.Navigation;

namespace ProjectX.Views
{
    public partial class RegisterWindow : Window
    {
        private readonly IMongoDatabase _database;

        public RegisterWindow()
        {
            InitializeComponent();
            _database = GetMongoDatabase();
        }



        private IMongoDatabase GetMongoDatabase()
        {
            string connectionString =
                "mongodb+srv://slowey:tlvptlvp@projectx.3vv2dfv.mongodb.net/";
            string databaseName = "ProjectX";

            var client = new MongoClient(connectionString);
            return client.GetDatabase(databaseName);
        }
        private bool CheckUsername(string username)
        {
            var usersCollection = _database.GetCollection<BsonDocument>("users");

            var filter = Builders<BsonDocument>.Filter.Eq("username", username);
            var count = usersCollection.CountDocuments(filter);

            return count > 0;
        }



        private bool Register(string username, string email, string zaloapi, string fptapi, string password)
        {
            var usersCollection = _database.GetCollection<BsonDocument>("users");

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            DateTime currentTimestamp = DateTime.Now;

            var document = new BsonDocument
            {
                { "username", username },
                { "email", email }, 
                { "useraccountname", BsonNull.Value },
                { "zaloapi", zaloapi },
                { "fptapi", fptapi },
                { "password", hashedPassword },
                { "created_at", currentTimestamp },
                { "last_used_at", currentTimestamp },
                { "premium", false  }
            };

            try
            {
                usersCollection.InsertOne(document);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }


        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string email = EmailTextBox.Text;
            string zaloapi = ZaloAI.Text;
            string fptapi = FPTAI.Text;
            string password = PasswordBox.Password;

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(zaloapi) && !string.IsNullOrEmpty(password))
            {
                if (!IsValidEmail(email))
                {
                    MessageBox.Show("Email không hợp lệ.");
                    return;
                }

                if (CheckUsername(username))
                {
                    MessageBox.Show("Tên tài khoản đã tồn tại.");
                }
                else
                {
                    bool isZaloKeyValid = await CheckZaloKeyAsync(zaloapi);
                    bool isFPTKeyValid = await CheckFPTKeyAsync(fptapi);

                    if (!isZaloKeyValid)
                    {
                        MessageBox.Show("API Key Zalo không hợp lệ.");
                        return;
                    }

                    if (!isFPTKeyValid)
                    {
                        MessageBox.Show("API Key FPT không hợp lệ.");
                        return;
                    }

                    if (!IsStrongPassword(password))
                    {
                        MessageBox.Show("Mật khẩu phải có ít nhất 8 ký tự.");
                        return;
                    }

                    if (Register(username, email, zaloapi, fptapi, password))
                    {
                        MessageBox.Show("Đăng ký thành công.");
                        LoginWindow loginWindow = new LoginWindow();
                        loginWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Đăng ký thất bại.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
            }
        }

        private bool IsStrongPassword(string password)
        {
            return password.Length >= 8;
        }



        private bool IsValidEmail(string email)
        {
            string emailPattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

            return System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern);
        }




        private void BackToLoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private async Task<bool> CheckZaloKeyAsync(string key)
        {
            var client = new RestClient("https://api.zalo.ai/v1/tts/synthesize");
            var request = new RestRequest(Method.POST);
            request.AddHeader("apikey", key);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            var response = await client.ExecuteAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return false;
            }

            return true;
        }

        private async Task<bool> CheckFPTKeyAsync(string key)
        {
            var client = new RestClient("https://api.fpt.ai/dmp/checklive/v2");
            var request = new RestRequest(Method.POST);
            request.AddHeader("api-key", key);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            var response = await client.ExecuteAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return false;
            }

            return true;
        }
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            // Mở liên kết trong trình duyệt mặc định
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));

            // Ngăn chặn chuyển hướng trong Hyperlink
            e.Handled = true;
        }
    }
}