using System.Windows;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ProjectX.Views
{
    public partial class ChangePasswordWindow : Window
    {
        private readonly string username;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<BsonDocument> usersCollection;

        public bool PasswordChanged { get; private set; }

        public ChangePasswordWindow(string username)
        {
            InitializeComponent();
            this.username = username;
            _database = GetMongoDatabase();
            usersCollection = _database.GetCollection<BsonDocument>("users");
        }

        private IMongoDatabase GetMongoDatabase()
        {
            string connectionString =
                "mongodb+srv://slowey:tlvptlvp@projectx.3vv2dfv.mongodb.net/";
            string databaseName = "ProjectX";

            var client = new MongoClient(connectionString);
            return client.GetDatabase(databaseName);
        }

        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            string oldPassword = OldPasswordBox.Password;
            string newPassword = NewPasswordBox.Password;

            if (VerifyOldPassword(oldPassword))
            {
                UpdateUserPassword(newPassword);
                PasswordChanged = true;
                MessageBox.Show("Thay đổi mật khẩu thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            else
            {
                MessageBox.Show("Mật khẩu cũ không đúng. Vui lòng thử lại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool VerifyOldPassword(string oldPassword)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("username", username);
            var user = usersCollection.Find(filter).FirstOrDefault();

            if (user != null)
            {
                return BCrypt.Net.BCrypt.Verify(oldPassword, user["password"].AsString);
            }

            return false;
        }

        private void UpdateUserPassword(string newPassword)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);

            var filter = Builders<BsonDocument>.Filter.Eq("username", username);
            var update = Builders<BsonDocument>.Update.Set("password", hashedPassword);

            usersCollection.UpdateOne(filter, update);
        }
    }
}
