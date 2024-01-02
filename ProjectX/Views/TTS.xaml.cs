using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IronOcr;
using MongoDB.Bson;
using MongoDB.Driver;
using RestSharp;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace ProjectX.Views
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class TTS : Window
    {
        private readonly string[] arrGiong = { "Nữ miền Nam", "Nữ miền Bắc", "Nam miền Nam", "Nam miền Bắc" };
        private readonly int[] arrGiongmini = { 1, 2, 3, 4 };
        private readonly Thread ThreadUpdateUI;
        private string _apikey;
        private CancellationTokenSource cancellationTokenSource;
        private string keylone = "";
        private AudioProcessor MainXuLy;
        private Thread ThreadBackround;

        public TTS(string username)
        {
            InitializeComponent();
            var apiKey = GetApiKeyByUsername(username);
            _apikey = apiKey;
            ThreadUpdateUI = new Thread(() => UpdateUI());
            ThreadUpdateUI.IsBackground = true;
            ThreadUpdateUI.Start();
        }

        private string GetApiKeyByUsername(string username)
        {
            var apiKey = "";

            var client = new MongoClient(
                    "mongodb+srv://slowey:tlvptlvp@projectx.3vv2dfv.mongodb.net/");
            var database = client.GetDatabase("ProjectX");
            var collection = database.GetCollection<BsonDocument>("users");

            var filter = Builders<BsonDocument>.Filter.Eq("username", username);
            var result = collection.Find(filter).FirstOrDefault();

            if (result != null) apiKey = result.GetValue("zaloapi").AsString;

            return apiKey;
        }


        private void ProcessFile(string filePath)
        {
            if (filePath.EndsWith(".txt"))
            {
                var extractedText = File.ReadAllText(filePath, Encoding.UTF8);
                _text.Text = extractedText;
            }
            else if (filePath.EndsWith(".jpg") || filePath.EndsWith(".png"))
            {
                var extractedText = PerformOcr(filePath);
                _text.Text = extractedText;
            }
        }

        private string PerformOcr(string imagePath)
        {
            var Ocr = new IronTesseract
            {
                Language = OcrLanguage.Vietnamese
            };
            var result = Ocr.Read(imagePath);
            return result.Text;
        }

        private void BtnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();

            var result = openFileDialog.ShowDialog();
            if (result == true) ProcessFile(openFileDialog.FileName);
        }

        private void UpdateUI()
        {
            var textPre = "";
            cancellationTokenSource = new CancellationTokenSource();

            Task.Run(() =>
            {
                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    Dispatcher.Invoke(() =>
                    {
                        if (_text.Text != textPre)
                        {
                            _kytu.Content = "Ký tự đã nhập: " + _text.Text.Length;
                            textPre = _text.Text;
                        }
                    });
                    Thread.Sleep(200);
                }
            }, cancellationTokenSource.Token);
        }


        private bool CheckKey(string key)
        {
            if (keylone == key) return true;

            _tientring.Content = "Đang kiểm tra API KEY";
            Thread.Sleep(500);
            var client = new RestClient("https://api.zalo.ai/v1/tts/synthesize");
            var request = new RestRequest(Method.POST);
            request.AddHeader("apikey", key);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            var response = client.Execute(request);
            if ((int)response.StatusCode == 401)
            {
                _tientring.Content = "Chưa khởi động.";
                return false;
            }

            _tientring.Content = "Đang xử lý";
            keylone = key;
            return true;
        }

        private void _run_Click(object sender, RoutedEventArgs e)
        {
            if (CheckKey(_apikey) == false)
            {
                MessageBox.Show("API KEY không đúng, bạn vui lòng kiểm tra lại key của mình rồi thử lại nhé!", "Lỗi");
            }
            else
            {
                var text = _text.Text;
                var gender = arrGiongmini[Array.IndexOf(arrGiong, _nguoidoc.Text)];
                var speed = StringBetween(_tocdo.Text, "(", ")");
                var apikey = _apikey;
                MainXuLy = new AudioProcessor(text, gender, speed, apikey);
                MainXuLy.MainRun();
                ThreadBackround = new Thread(() => Backround());
                ThreadBackround.IsBackground = true;
                ThreadBackround.Start();
            }
        }

        private void _stop_Click(object sender, RoutedEventArgs e)
        {
            if (MainXuLy != null)
            {
                MainXuLy.StopRead();
                MainXuLy.StopDown();
            }
        }


        private void Grid_MouseLeftButtonDown(object sender, TouchEventArgs e)
        {
            var move = sender as Grid;
            var win = GetWindow(move);
            win.DragMove();
        }

        public string StringBetween(string STR, string FirstString, string LastString)
        {
            string FinalString;
            var Pos1 = STR.IndexOf(FirstString) + FirstString.Length;
            var Pos2 = STR.IndexOf(LastString);
            FinalString = STR.Substring(Pos1, Pos2 - Pos1);
            return FinalString;
        }

        private void _download_Click(object sender, RoutedEventArgs e)
        {
            if (CheckKey(_apikey) == false)
            {
                MessageBox.Show("API KEY không đúng, bạn vui lòng kiểm tra lại key của mình rồi thử lại nhé!", "Lỗi");
            }
            else
            {
                var text = _text.Text;
                var gender = arrGiongmini[Array.IndexOf(arrGiong, _nguoidoc.Text)];
                var speed = StringBetween(_tocdo.Text, "(", ")");
                var apikey = _apikey;
                MainXuLy = new AudioProcessor(text, gender, speed, apikey);
                MainXuLy.MainDown();
                ThreadBackround = new Thread(() => Backround());
                ThreadBackround.IsBackground = true;
                ThreadBackround.Start();
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void _back_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void Window_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void _text_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_kytu != null) _kytu.Content = "Ký tự đã nhập: " + _text.Text.Length;
        }

        private void _text_TextChanged_1(object sender, TextChangedEventArgs e)
        {
        }

        private void Backround()
        {
            while (true)
            {
                Dispatcher.Invoke(() => { _tientring.Content = MainXuLy.GetProcessMes(); });

                Thread.Sleep(2000);
            }
        }
    }
}