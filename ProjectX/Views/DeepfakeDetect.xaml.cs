using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Windows.Media.Imaging;

namespace ProjectX.Views
{
    public partial class DeepfakeDetect : Window
    {
        private string apiKey;
        private string videoFilePath;
        private string imageFilePath;
        private readonly IMongoCollection<User> _usersCollection;

        public DeepfakeDetect(string username)
        {
            InitializeComponent();
            _usersCollection = GetUsersCollection();
            apiKey = GetApiKeyByUsername(username);
        }

        private void UpdateVideoView(string videoFilePath)
        {
            VideoPlayer.Source = new Uri(videoFilePath);
            VideoPlayer.Visibility = Visibility.Visible;
        }

        private void UpdateImageView(string imageFilePath)
        {
            ImageView.Source = new BitmapImage(new Uri(imageFilePath));
            ImageView.Visibility = Visibility.Visible;
        }

        private void ResetViews()
        {
            //VideoPlayer.Visibility = Visibility.Hidden;
            //ImageView.Visibility = Visibility.Hidden;
        }

        private void UploadVideo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Filter = "Video files (*.mp4)|*.mp4";
            dialog.Multiselect = false;
            var result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                videoFilePath = dialog.FileName;
                VideoFileName.Text = $"Video file: {videoFilePath}";

                // Update video view
                UpdateVideoView(videoFilePath);

                // Reset image view
            }
        }

        private void UploadImage_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Filter = "Image files (*.jpg, *.png)|*.jpg;*.png";
            dialog.Multiselect = false;
            var result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                imageFilePath = dialog.FileName;
                ImageFileName.Text = $"Image file: {imageFilePath}";

                // Update image view
                UpdateImageView(imageFilePath);

                // Reset video view
            }
        }


        private IMongoCollection<User> GetUsersCollection()
        {
            string connectionString =
                "mongodb+srv://slowey:tlvptlvp@projectx.3vv2dfv.mongodb.net/";
            string databaseName = "ProjectX";
            string collectionName = "users";

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            return database.GetCollection<User>(collectionName);
        }
        private async void StartDetection_Click(object sender, RoutedEventArgs e)
        {
            var result = await DetectDeepfake();
            ResultText.Text = $"Detection result: {result}";
        }

        private async Task<string> DetectDeepfake()
        {
            var url = "https://api.fpt.ai/dmp/checklive/v2";
            var client = new HttpClient();
            var content = new MultipartFormDataContent
    {
        { new StreamContent(File.OpenRead(videoFilePath)), "video", "video.mp4" },
        { new StreamContent(File.OpenRead(imageFilePath)), "cmnd", "face.jpg" }
    };

            content.Headers.Add("api-key", apiKey);

            var response = await client.PostAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();

            var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseString);

            return InterpretResponse(jsonResponse);
        }

        private string InterpretResponse(dynamic response)
        {
            var resultBuilder = new StringBuilder();

            if (response.code != "200")
            {
                resultBuilder.AppendLine($"Error: {response.message} (Code: {response.code})");
                if (response.data != null)
                {
                    resultBuilder.AppendLine($"Data: {response.data}");
                }
                return resultBuilder.ToString();
            }

            resultBuilder.AppendLine($"Detection result: {response.message}");

            if (response.is_live != null)
            {
                resultBuilder.AppendLine($"Live: {response.is_live} (Probability: {response.prob})");
            }

            if (response.need_to_review != null)
            {
                resultBuilder.AppendLine($"Need to review: {response.need_to_review}");
            }

            if (response.is_deepfake != null)
            {
                resultBuilder.AppendLine($"Deepfake: {response.is_deepfake} (Probability: {response.deepfake_prob})");
            }

            if (response.face_match != null)
            {
                resultBuilder.AppendLine($"Face Match: {response.face_match.isMatch} (Similarity: {response.face_match.similarity}%)");
            }
            else if (response.face_match_error != null)
            {
                resultBuilder.AppendLine($"Face Match Error: {response.face_match_error.data}");
            }

            return resultBuilder.ToString();
        }

        private string GetApiKeyByUsername(string username)
        {


            var filter = Builders<User>.Filter.Eq(u => u.username, username);
            var user = _usersCollection.Find(filter).FirstOrDefault();

            if (user != null)
            {
                return user.fptapi;
            }

            return null;
        }

        // update video and image view 

    }
}

