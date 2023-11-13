using Newtonsoft.Json;
using RestSharp;
using System.Data.SqlClient;
using System.Windows;

namespace ProjectX
{
    public partial class RegisterWindow : Window
    {
        string connectionString = Properties.Settings.Default.connection_string;

        public RegisterWindow()
        {
            InitializeComponent();
        }

        // This method checks if the API key is valid.
        // It returns true if the API key is valid, otherwise it returns false.
        private bool CheckKey(string key)
        {
            var client = new RestClient("https://api.zalo.ai/v1/tts/synthesize");
            var request = new RestRequest(Method.POST);
            request.AddHeader("apikey", key);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("input", "Kiểm tra API key"); // Add sample data

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var jsonResponse = JsonConvert.DeserializeObject<dynamic>(response.Content);
                int errorCode = jsonResponse.error_code;

                return errorCode == 0; // Key is valid if errorCode is 0
            }

            return false; // Return false if there is an error or errorCode is not 0
        }

        // This method checks if the username is already taken.
        // It returns true if the username is already taken, otherwise it returns false.
        private bool CheckUsername(string username)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Username = @Username", connection);
                command.Parameters.AddWithValue("@Username", username);

                int count = (int)command.ExecuteScalar();

                return count > 0; // Username exists if count > 0
            }
        }

        // This method registers a new user.
        // It returns true if the registration is successful, otherwise it returns false.
        private bool Register(string username, string apiKey, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Hash the password using bcrypt
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                SqlCommand command = new SqlCommand("INSERT INTO Users (Username, ApiKey, password) VALUES (@Username, @ApiKey, @password)", connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@ApiKey", apiKey);
                command.Parameters.AddWithValue("@password", hashedPassword);

                int result = command.ExecuteNonQuery();

                if (result > 0)
                {
                    return true; // Registration successful if result > 0
                }
            }
            return false; // Registration failed if result = 0
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string apiKey = ApiKeyTextBox.Text;
            string password = PasswordBox.Password;

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(apiKey) && !string.IsNullOrEmpty(password))
            {
                if (CheckUsername(username))
                {
                    MessageBox.Show("Username already taken.");
                }
                else
                {
                    if (CheckKey(apiKey))
                    {
                        if (Register(username, apiKey, password))
                        {
                            MessageBox.Show("Registration successful.");
                            LoginWindow loginWindow = new LoginWindow();
                            loginWindow.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Registration failed.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("API Key is invalid.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please enter a username, an API key, and a password.");
            }
        }

        private void BackToLoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the current window and open the login window
            this.Close();
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
        }
    }
}
