using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using RestSharp;
using IronOcr;

using MessageBox = System.Windows.MessageBox;
using Microsoft.Win32;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace ProjectX
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///

    public partial class MainWindow
    {
        string path = Directory.GetCurrentDirectory();
        string[] arrGiong = { "Nữ miền Nam", "Nữ miền Bắc", "Nam miền Nam", "Nam miền Bắc" };
        int[] arrGiongmini = { 1, 2, 3, 4 };
        int filexong = -1;
        string version = "3.0.1";
        Thread ThreadBackround;
        Thread DocThread, ThreadUpdateUI;
        Process ffmpeg;
        XuLyAmThanh MainXuLy;
        string keylone = "";

        public MainWindow(string apiKey)
        {
            InitializeComponent();
            _apikey.Text = apiKey;
            ThreadUpdateUI = new Thread(() => UpdateUI());
            ThreadUpdateUI.IsBackground = true;
            ThreadUpdateUI.Start();

        }
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var move = sender as System.Windows.Controls.Grid;
            var win = Window.GetWindow(move);
            win.DragMove();
        }


        private void ProcessFile(string filePath)
        {
            if (filePath.EndsWith(".txt"))
            {
                string extractedText = File.ReadAllText(filePath, Encoding.UTF8);
                _text.Text = extractedText;
            }
            else if (filePath.EndsWith(".jpg") || filePath.EndsWith(".png"))
            {
                string extractedText = PerformOcr(filePath);
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
            OpenFileDialog openFileDialog = new OpenFileDialog();

            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                ProcessFile(openFileDialog.FileName);
            }
        }

        private void UpdateUI()
        {
            string textPre = "";
            while (true)
            {
                this.Dispatcher.Invoke(() =>
                {

                    if (_text.Text != textPre)
                    {
                        _kytu.Content = "Ký tự đã nhập: " + _text.Text.Length.ToString();
                        textPre = _text.Text;

                    }
                });
                Thread.Sleep(200);
            }
        }
        private bool CheckKey(string key)
        {
            if (keylone == key)
            {
                return true;
            }
            else
            {
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


        }
        private void _run_Click(object sender, RoutedEventArgs e)
        {
            if (CheckKey(_apikey.Text) == false)
            {
                MessageBox.Show("API KEY không đúng, bạn vui lòng kiểm tra lại key của mình rồi thử lại nhé!", "Lỗi");
            }
            else
            {
                File.WriteAllText("APIKey.txt", _apikey.Text);
                string text = _text.Text;
                int gender = arrGiongmini[Array.IndexOf(arrGiong, _nguoidoc.Text)];
                string speed = StringBetween(_tocdo.Text, "(", ")");
                string apikey = _apikey.Text;
                MainXuLy = new XuLyAmThanh(text, gender, speed, apikey);
                MainXuLy.mainRun();
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
            var move = sender as System.Windows.Controls.Grid;
            var win = Window.GetWindow(move);
            win.DragMove();
        }

        public string StringBetween(string STR, string FirstString, string LastString)
        {
            string FinalString;
            int Pos1 = STR.IndexOf(FirstString) + FirstString.Length;
            int Pos2 = STR.IndexOf(LastString);
            FinalString = STR.Substring(Pos1, Pos2 - Pos1);
            return FinalString;
        }

        private void _download_Click(object sender, RoutedEventArgs e)
        {
            if (CheckKey(_apikey.Text) == false)
            {
                MessageBox.Show("API KEY không đúng, bạn vui lòng kiểm tra lại key của mình rồi thử lại nhé!", "Lỗi");
            }
            else
            {
                File.WriteAllText("APIKey.txt", _apikey.Text);
                string text = _text.Text;
                int gender = arrGiongmini[Array.IndexOf(arrGiong, _nguoidoc.Text)];
                string speed = StringBetween(_tocdo.Text, "(", ")");
                string apikey = _apikey.Text;
                MainXuLy = new XuLyAmThanh(text, gender, speed, apikey);
                MainXuLy.mainDown();
                ThreadBackround = new Thread(() => Backround());
                ThreadBackround.IsBackground = true;
                ThreadBackround.Start();
            }
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            //var client = new RestClient("https://raw.githubusercontent.com/sloweyyy/IT008.O12/main/ProjectX/version.txt");
            //var request = new RestRequest(Method.GET);
            //IRestResponse response = client.Execute(request);
            //if (!response.Content.Contains(version))
            //{
            //    MessageBox.Show("Đã có phiên bản mới. Hãy cập nhật nhé!");
            //    System.Diagnostics.Process.Start("https://github.com/sloweyyy/IT008.O12/releases/");
            //    System.Environment.Exit(1);
            //}
            //else
            //{

            //    _apikey.Text = System.IO.File.ReadAllText("APIKey.txt");
            //}

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
                this.DragMove();
        }

        private void _text_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (_kytu != null)
            {
                _kytu.Content = "Ký tự đã nhập: " + _text.Text.Length.ToString();
            }
        }


        private void Backround()
        {
            while (true)
            {
                this.Dispatcher.Invoke(() =>
                {
                    _tientring.Content = MainXuLy.getProcessMes();
                    _process.Value = MainXuLy.getProcessNow();

                });

                Thread.Sleep(2000);
            }
        }
    }
}