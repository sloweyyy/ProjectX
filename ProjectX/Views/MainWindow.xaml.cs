using System.Windows;
using System.Windows.Forms;
using ProjectX.Views;
using Application = System.Windows.Application;
using System;

namespace ProjectX
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///

    public partial class MainWindow
    {
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


    }
}