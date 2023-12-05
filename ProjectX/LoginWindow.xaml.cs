using System;
using System.Text.Json;
using System.Windows;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ProjectX
{
    public partial class LoginWindow : Window
    {
        private readonly IMongoCollection<BsonDocument> _usersCollection;

        public LoginWindow()
        {
            InitializeComponent();
            _usersCollection = GetMongoCollection(); // Initialize the MongoDB collection
        }

        private IMongoCollection<BsonDocument> GetMongoCollection()
        {
            // Set your MongoDB connection string and database name
            string connectionString = "mongodb+srv://slowey:tlvptlvp@projectx.3vv2dfv.mongodb.net/"; // Update with your MongoDB server details
            string databaseName = "ProjectX"; // Update with your database name
            string collectionName = "users"; // Update with your collection name

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<BsonDocument>(collectionName);
        }

        private void CheckLogin_Click(object sender, RoutedEventArgs e)
        {
            string selectedUsername = UserComboBox.SelectedItem as string;
            string enteredPassword = PasswordBox.Password;

            if (selectedUsername != null && !string.IsNullOrEmpty(enteredPassword))
            {
                var user = GetUser(selectedUsername);

                if (user == null)
                {
                    MessageBox.Show("Sai tên tài khoản hoặc mật khẩu.");
                    return;
                }

                string storedPasswordHash = user.GetValue("password").AsString;

                if (VerifyPassword(enteredPassword, storedPasswordHash))
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

        private BsonDocument GetUser(string username)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("username", username);
            return _usersCollection.Find(filter).FirstOrDefault();
        }

        private bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            // Implement password verification logic here (e.g., using BCrypt)
            // You can use BCrypt.Net or any other library to verify passwords
            // Example: return BCrypt.Net.BCrypt.Verify(enteredPassword, storedPasswordHash);
            // Make sure to add the necessary NuGet packages for BCrypt.Net or your chosen library.
            throw new NotImplementedException("Implement password verification logic here.");
        }

        private void LoadUsers()
        {
            UserComboBox.Items.Clear();
            using (var cursor = _usersCollection.Find(new BsonDocument()).ToCursor())
            {
                foreach (var document in cursor.ToEnumerable())
                {
                    UserComboBox.Items.Add(document.GetValue("username").AsString);
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
