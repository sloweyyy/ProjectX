using IronOcr;
using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace ProjectX.Views
{
    public partial class OCR : Window
    {
        public OCR()
        {
            InitializeComponent();
        }

        private void BtnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                ProcessFile(openFileDialog.FileName);
            }
        }

        private void ProcessFile(string filePath)
        {
            if (filePath.EndsWith(".txt"))
            {
                string extractedText = File.ReadAllText(filePath, Encoding.UTF8);
                TextContent.Text = extractedText;
            }
            else if (filePath.EndsWith(".jpg") || filePath.EndsWith(".png"))
            {
                string extractedText = PerformOcr(filePath);
                TextContent.Text = extractedText;
            }
            CharCountLabel.Content = "Characters: " + TextContent.Text.Length;
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

        private void TextContent_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CharCountLabel.Content = "Characters: " + TextContent.Text.Length;
        }

        private void BtnDownloadFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt";
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, TextContent.Text);
            }

        }
    }
}
