using System.Windows;
using System.Windows.Forms;
using ProjectX.Views;
using Application = System.Windows.Application;
using System;
using System.IO;
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

        public MainWindow(string username)
        {
            InitializeComponent();
            _username = username;
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
            if (!IsWindowOpen(typeof(OCR)))
            {
                OCR ocr = new OCR();
                ocr.Show();
            }
        }

        private void DeepfakeDetect_Click(object sender, RoutedEventArgs e)
        {
            if (!IsWindowOpen(typeof(DeepfakeDetect)))
            {
                DeepfakeDetect deepfakeDetect = new DeepfakeDetect(_username);
                deepfakeDetect.Show();
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

    }
}