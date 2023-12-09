using System.Windows;
using ProjectX.Views;

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
            TTS tts = new TTS(_username);
            tts.Show();
            this.Close();
        }

        private void OCR_Click(object sender, RoutedEventArgs e)
        {
            OCR ocr = new OCR();
            ocr.Show();
            this.Close();
        }

        private void DeepfakeDetect_Click(object sender, RoutedEventArgs e)
        {
            DeepfakeDetect deepfakeDetect = new DeepfakeDetect(_username);
            deepfakeDetect.Show();
            this.Close();
        }

    }
}