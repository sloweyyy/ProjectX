using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace ProjectX.Views
{
    public partial class FaceMatch : Window
    {
        private readonly IMongoCollection<User> _usersCollection;
        private readonly string apiKey;
        private string imageFilePath1;
        private string imageFilePath2;

        public FaceMatch(string username)
        {
            InitializeComponent();
            _usersCollection = GetUsersCollection();
            apiKey = GetApiKeyByUsername(username);
        }

        private void UpdateImageView1(string filePath)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(filePath);
            bitmap.EndInit();
            ImageView1.Source = bitmap;
        }

        private void UpdateImageView2(string filePath)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(filePath);
            bitmap.EndInit();
            ImageView2.Source = bitmap;
        }

        private void UploadImage_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Image files (*.jpg, *.png)|*.jpg;*.png";
            dialog.Multiselect = false;
            var result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                var imageFilePath = dialog.FileName;

                if (sender == UploadImage1)
                {
                    imageFilePath1 = imageFilePath;
                    ImageFileName1.Text = $"File ảnh: {imageFilePath1}";
                    UpdateImageView1(imageFilePath1);
                }
                else if (sender == UploadImage2)
                {
                    imageFilePath2 = imageFilePath;
                    ImageFileName2.Text = $"File ảnh: {imageFilePath2}";
                    UpdateImageView2(imageFilePath2);
                }
            }
        }

        private async void StartDetection_Click(object sender, RoutedEventArgs e)
        {
            var result = await DetectFaceMatch();
            MatchResultText.Text = $"Kết quả phát hiện: {result}";
        }

        private async Task<string> DetectFaceMatch()
        {
            var url = "https://api.fpt.ai/dmp/checkface/v1";
            var client = new HttpClient();
            var content = new MultipartFormDataContent();

            client.DefaultRequestHeaders.Add("api_key", apiKey);

            content.Add(new StreamContent(File.OpenRead(imageFilePath1)), "file[]", "image1.jpg");
            content.Add(new StreamContent(File.OpenRead(imageFilePath2)), "file[]", "image2.jpg");

            var response = await client.PostAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();

            var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseString);
            return InterpretFaceMatchResponse(jsonResponse);
        }

        private string InterpretFaceMatchResponse(dynamic response)
        {
            if (response.code != "200") return $"Lỗi: {response.message} (Mã: {response.code})";

            if (response.data != null && response.data.isMatch != null && response.data.similarity != null)
                return $"Khớp khuôn mặt: {response.data.isMatch} (Tương đồng: {response.data.similarity}%)";

            return "Không tìm thấy thông tin khớp khuôn mặt trong phản hồi.";
        }

        private IMongoCollection<User> GetUsersCollection()
        {
            var connectionString = "mongodb+srv://slowey:tlvptlvp@projectx.3vv2dfv.mongodb.net/";
            var databaseName = "ProjectX";
            var collectionName = "users";

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            return database.GetCollection<User>(collectionName);
        }

        private string GetApiKeyByUsername(string username)
        {
            var filter = Builders<User>.Filter.Eq(u => u.username, username);
            var user = _usersCollection.Find(filter).FirstOrDefault();
            return user?.fptapi;
        }
    }
}
