using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace ProjectX
{
    public partial class LoginWindow : Window
    {
        string connectionString = Properties.Settings.Default.connection_string;

        public LoginWindow()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void CheckLogin_Click(object sender, RoutedEventArgs e)
        {
            string selectedUsername = UserComboBox.SelectedItem as string;
            string enteredPassword = PasswordBox.Password;

            if (selectedUsername != null && !string.IsNullOrEmpty(enteredPassword))
            {
                string storedPasswordHash = GetPasswordHash(selectedUsername);

                if (storedPasswordHash != null && VerifyPassword(enteredPassword, storedPasswordHash))
                {
                    // Password is valid, grant access
                    MessageBox.Show("Đăng nhập thành công.");
                    // You can open the main window or perform other actions here.
                    // open the main window
                    // public MainWindow(string username)
                    MainWindow mainWindow = new MainWindow(selectedUsername);
                    mainWindow.Show();

                    // Close the login window
                    this.Close();
                }
                else
                {
                    // Password is invalid
                    MessageBox.Show("Mật khẩu không đúng.");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn tài khoản và nhập mật khẩu.");
            }
        }

        private string GetPasswordHash(string username)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT password FROM Users WHERE username = @username", connection);
                command.Parameters.AddWithValue("@username", username);

                object result = command.ExecuteScalar();

                if (result != null)
                {
                    return result.ToString();
                }
            }

            return null;
        }

        private bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedPasswordHash);
        }


        private void LoadUsers()
        {
            UserComboBox.Items.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT username FROM Users", connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UserComboBox.Items.Add(reader["username"].ToString());
                    }
                }
            }
        }

        private void OpenRegisterForm_Click(object sender, RoutedEventArgs e)
        {
            // Open the register window
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.Show();

            // Close the login window
            this.Close();
        }
    }
}
