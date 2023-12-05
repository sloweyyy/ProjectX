using MongoDB.Driver;
using System.Windows.Controls;
using System.Windows;
using RestSharp;
using System.IO;

namespace ProjectX.Views
{
    public partial class LoginWindow : Window
    {
        private readonly IMongoCollection<User> _usersCollection;
        private string version = File.ReadAllText("..\\..\\version.txt");
        public LoginWindow()
        {
            InitializeComponent();
            _usersCollection = GetMongoCollection(); 
            Loaded += Window_Loaded;
        }

        private IMongoCollection<User> GetMongoCollection()
        {
            string connectionString =
                "mongodb+srv://slowey:tlvptlvp@projectx.3vv2dfv.mongodb.net/"; 
            string databaseName = "ProjectX"; 

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<User>("users");
        }

        private void CheckLogin_Click(object sender, RoutedEventArgs e)
        {
            var selectedUsername = UsernameTextBox.Text;
            var enteredPassword = PasswordBox.Password;

            if (selectedUsername != null && !string.IsNullOrEmpty(enteredPassword))
            {
                var user = GetUser(selectedUsername);

                if (user == null)
                {
                    MessageBox.Show("Sai tên tài khoản hoặc mật khẩu.");
                    return;
                }

                if (VerifyPassword(enteredPassword, user.password))
                {
                    MainWindow mainWindow = new MainWindow(selectedUsername);
                    mainWindow.Show();

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Sai tên tài khoản hoặc mật khẩu.");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
            }
        }

        private User GetUser(string username)
        {
            var filter = Builders<User>.Filter.Eq(u => u.username, username);
            return _usersCollection.Find(filter).FirstOrDefault();
        }

        private bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedPasswordHash);
        }


        private void OpenRegisterForm_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.Show();

            this.Close();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            var client = new RestClient("https://raw.githubusercontent.com/sloweyyy/IT008.O12/main/ProjectX/version.txt");

            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            if (!response.Content.Contains(version))
            {
                MessageBox.Show("Đã có phiên bản mới. Hãy cập nhật nhé!");
                System.Diagnostics.Process.Start("https://github.com/sloweyyy/IT008.O12/releases");
            }



        }


    }
}