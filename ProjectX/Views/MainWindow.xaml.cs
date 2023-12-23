using System.Windows;
using System.Windows.Forms;
using ProjectX.Views;
using Application = System.Windows.Application;
using System;
using System.IO;
using MongoDB.Driver;
using RestSharp;
using MessageBox = System.Windows.MessageBox;

namespace ProjectX
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///

    public partial class MainWindow
    {
        private string currentVersion = File.ReadAllText("..\\..\\version.txt");
        private string _username;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<User> usersCollection;


        public MainWindow(string username)
        {
            InitializeComponent();
            _username = username;
            _database = GetMongoDatabase();
            if (_database == null)
            {
                Console.WriteLine("Failed to connect to the database.");
                return;
            }
            usersCollection = _database.GetCollection<User>("users");
            User currentUser = GetUserByUsername(_username);
            if (currentUser != null)
            {
                GeminiButton.IsEnabled = currentUser.premium;
                GeminiButton.Opacity = currentUser.premium ? 1 : 0.5;
            }
        }
        private IMongoDatabase GetMongoDatabase()
        {
            string connectionString =
                "mongodb+srv://slowey:tlvptlvp@projectx.3vv2dfv.mongodb.net/";
            string databaseName = "ProjectX";

            var client = new MongoClient(connectionString);
            return client.GetDatabase(databaseName);
        }


        private void TTS_Click(object sender, RoutedEventArgs e)
        {

            if (!IsWindowOpen(typeof(TTS)))
            {
                TTS tts = new TTS(_username);
                tts.Show();
            }

        }

        private void OCR_Click(object sender, RoutedEventArgs e)
        {
            if (!IsWindowOpen(typeof(Translator)))
            {
                Translator translator = new Translator();
                translator.Show();
            }
        }

        private void FaceMatchClick(object sender, RoutedEventArgs e)
        {
            if (!IsWindowOpen(typeof(FaceMatch)))
            {
                FaceMatch faceMatch = new FaceMatch(_username);
                faceMatch.Show();
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
        public bool IsWindowOpen(Type windowType)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == windowType)
                {
                    return true;
                }
            }
            return false;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            if (!IsWindowOpen(typeof(About)))
            {   
                About about = new About();
                about.Show();
            }
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

        private void TermCondition_Click(object sender, RoutedEventArgs e)
        {
            if (!IsWindowOpen(typeof(TermsandCondition)))
            {
                TermsandCondition termCondition = new TermsandCondition();
                termCondition.Show();
            }
        }

        private void Gemini_Click(object sender, RoutedEventArgs e)
        {
            if (!IsWindowOpen(typeof(Gemini)))
            {
                Gemini gemini = new Gemini();
                gemini.Show();
            }
        }

        private void Account_Click(object sender, RoutedEventArgs e)
        {
            if (!IsWindowOpen(typeof(Account)))
            {
                // Fetch the user object based on the username
                User currentUser = GetUserByUsername(_username);

                if (currentUser != null)
                {
                    // Pass the user object to the Account constructor
                    Account account = new Account(currentUser);
                    account.Show();
                }
                else
                {
                    MessageBox.Show("User not found.");
                }
            }
        }

        private User GetUserByUsername(string username)
        {
            var filter = Builders<User>.Filter.Eq("username", username);
            return usersCollection.Find(filter).FirstOrDefault();
            
        }



    }
}