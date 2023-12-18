using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Windows;

namespace ProjectX.Views
{
    public partial class Account : Window
    {
        private User user;
        private IMongoDatabase _database;
        private IMongoCollection<User> usersCollection;

        public Account(User user)
        {
            InitializeComponent();
            this.user = user;
            _database = GetMongoDatabase();
            if (_database == null)
            {
                Console.WriteLine("Failed to connect to the database.");
                return;
            }
            usersCollection = _database.GetCollection<User>("users");

            DataContext = user;
        }


        private IMongoDatabase GetMongoDatabase()
        {
            string connectionString = "mongodb+srv://slowey:tlvptlvp@projectx.3vv2dfv.mongodb.net/";
            string databaseName = "ProjectX";

            var client = new MongoClient(connectionString);
            return client.GetDatabase(databaseName);
        }

        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePasswordWindow changePasswordWindow = new ChangePasswordWindow(user.username);
            changePasswordWindow.ShowDialog();
        }

        private void UpdateAccountNameButton_Click(object sender, RoutedEventArgs e)
        {
            string newAccountName = _accountNameTextBox.Text;


            UpdateUserAccountName(newAccountName);

            MessageBox.Show("Account name updated successfully!");
            _accountNameTextBox.Text = newAccountName;
        }

        private void UpdateUserAccountName(string newAccountName)
        {
            var filter = Builders<User>.Filter.Eq(u => u.username, user.username);
            var update = Builders<User>.Update.Set(u => u.useraccountname, newAccountName);
            usersCollection.UpdateOne(filter, update);
        }
    }
}
