using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using BCrypt.Net;
using RestSharp;
using System.Windows.Controls;
using System.Windows;

namespace ProjectX.Views
{
    public partial class RegisterWindow : Window
    {
        private readonly IMongoDatabase _database;

        public RegisterWindow()
        {
            InitializeComponent();
            _database = GetMongoDatabase(); // Initialize the MongoDB database connection
        }

        private IMongoDatabase GetMongoDatabase()
        {
            // Set your MongoDB connection string and database name
            string connectionString =
                "mongodb+srv://slowey:tlvptlvp@projectx.3vv2dfv.mongodb.net/"; // Update with your MongoDB server details
            string databaseName = "ProjectX"; // Update with your database name

            var client = new MongoClient(connectionString);
            return client.GetDatabase(databaseName);
        }

        // This method checks if the username is already taken.
        // It returns true if the username is already taken, otherwise it returns false.
        private bool CheckUsername(string username)
        {
            var usersCollection = _database.GetCollection<BsonDocument>("users");

            var filter = Builders<BsonDocument>.Filter.Eq("username", username);
            var count = usersCollection.CountDocuments(filter);

            return count > 0; // Username exists if count > 0
        }

        // This method registers a new user.
        // It returns true if the registration is successful, otherwise it returns false.
        private bool Register(string username, string apiKey, string password)
        {
            var usersCollection = _database.GetCollection<BsonDocument>("users");

            // Hash the password using bcrypt
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var document = new BsonDocument
            {
                { "username", username },
                { "apikey", apiKey },
                { "password", hashedPassword }
            };

            try
            {
                usersCollection.InsertOne(document);
                return true; // Registration successful
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during registration
                Console.WriteLine(ex.Message);
                return false; // Registration failed
            }
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string apiKey = ApiKeyTextBox.Text;
            string password = PasswordBox.Password;

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(apiKey) && !string.IsNullOrEmpty(password))
            {
                if (CheckUsername(username))
                {
                    // Username is already taken
                    MessageBox.Show("Tên tài khoản đã tồn tại.");
                }
                else
                {
                    bool isKeyValid = await CheckKeyAsync(apiKey);
                    if (!isKeyValid)
                    {
                        MessageBox.Show("API Key không hợp lệ.");
                        return;
                    }

                    if (Register(username, apiKey, password))
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


        private void BackToLoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the current window and open the login window
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private async Task<bool> CheckKeyAsync(string key)
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


        
    }
}