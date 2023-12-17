using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using IronOcr;
using Microsoft.Win32;

namespace ProjectX.Views
{
    public partial class OCR : Window
    {
        private readonly HttpClient httpClient = new HttpClient();
        private string currentLanguage;

        public OCR()
        {
            InitializeComponent();
            currentLanguage = GetLanguageCodeFromComboBox();
        }

        private async void BtnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true) await ProcessFileAsync(openFileDialog.FileName);
        }

        private async Task ProcessFileAsync(string filePath)
        {
            try
            {
                if (filePath.EndsWith(".txt"))
                {
                    var extractedText = File.ReadAllText(filePath, Encoding.UTF8);
                    TextContent.Text = extractedText;
                }
                else if (filePath.EndsWith(".jpg") || filePath.EndsWith(".png"))
                {
                    var extractedText = await PerformOcrAsync(filePath);
                    TextContent.Text = extractedText;
                }

                CharCountLabel.Content = "Characters: " + TextContent.Text.Length;
                LanguageSelection.SelectedIndex = 0;
                currentLanguage = GetLanguageCodeFromComboBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing file: {ex.Message}");
            }
        }

        private async Task<string> PerformOcrAsync(string imagePath)
        {
            var Ocr = new IronTesseract
            {
                Language = OcrLanguage.Vietnamese
            };
            var result = await Ocr.ReadAsync(imagePath);
            return result.Text;
        }

        private void TextContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            CharCountLabel.Content = "Characters: " + TextContent.Text.Length;
        }

        private void BtnDownloadFile_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Text file (*.txt)|*.txt"
            };

            if (saveFileDialog.ShowDialog() == true)
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, TextContent.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving file: {ex.Message}");
                }
        }

        private async void BtnTranslate_Click(object sender, RoutedEventArgs e)
        {
            await TranslateTextAndDisplayAsync();
        }

        private async Task TranslateTextAndDisplayAsync()
        {
            if (!string.IsNullOrWhiteSpace(TextContent.Text))
            {
                var selectedLanguage = GetLanguageCodeFromComboBox();
                TextContent.Text = await TranslateTextAsync(TextContent.Text, currentLanguage, selectedLanguage);
                currentLanguage = selectedLanguage; // Update currentLanguage
            }
        }

        private async Task<string> TranslateTextAsync(string input, string sourceLang, string targetLang)
        {
            var url =
                $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={sourceLang}&tl={targetLang}&dt=t&q={Uri.EscapeUriString(input)}";

            try
            {
                var result = await httpClient.GetStringAsync(url);
                var jsonData = new JavaScriptSerializer().Deserialize<List<dynamic>>(result);
                var translationItems = jsonData[0];
                var translation = "";
                foreach (object item in translationItems)
                {
                    var translationLineObject = item as IEnumerable;
                    var translationLineString = translationLineObject.GetEnumerator();
                    translationLineString.MoveNext();
                    translation += string.Format("{0}", Convert.ToString(translationLineString.Current));
                }

                if (!string.IsNullOrEmpty(translation))
                    // Remove any leading whitespace
                    translation = translation.TrimStart();

                return translation;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Translation failed. Error: {ex.Message}");
                return input;
            }
        }

        private string GetLanguageCodeFromComboBox()
        {
            var languageCodes = new Dictionary<string, string>
            {
                { "Afrikaans", "af" },
                { "Albanian", "sq" },
                { "Amharic", "am" },
                { "Arabic", "ar" },
                { "Armenian", "hy" },
                { "Azerbaijani", "az" },
                { "Basque", "eu" },
                { "Belarusian", "be" },
                { "Bengali", "bn" },
                { "Bosnian", "bs" },
                { "Bulgarian", "bg" },
                { "Catalan", "ca" },
                { "Cebuano", "ceb" },
                { "Chichewa", "ny" },
                { "Chinese (Simplified)", "zh-cn" },
                { "Chinese (Traditional)", "zh-tw" },
                { "Corsican", "co" },
                { "Croatian", "hr" },
                { "Czech", "cs" },
                { "Danish", "da" },
                { "Dutch", "nl" },
                { "English", "en" },
                { "Esperanto", "eo" },
                { "Estonian", "et" },
                { "Filipino", "tl" },
                { "Finnish", "fi" },
                { "French", "fr" },
                { "Frisian", "fy" },
                { "Galician", "gl" },
                { "Georgian", "ka" },
                { "German", "de" },
                { "Greek", "el" },
                { "Gujarati", "gu" },
                { "Haitian Creole", "ht" },
                { "Hausa", "ha" },
                { "Hawaiian", "haw" },
                { "Hebrew", "iw" }, 
                { "Hindi", "hi" },
                { "Hmong", "hmn" },
                { "Hungarian", "hu" },
                { "Icelandic", "is" },
                { "Igbo", "ig" },
                { "Indonesian", "id" },
                { "Irish", "ga" },
                { "Italian", "it" },
                { "Japanese", "ja" },
                { "Javanese", "jw" },
                { "Kannada", "kn" },
                { "Kazakh", "kk" },
                { "Khmer", "km" },
                { "Korean", "ko" },
                { "Kurdish (Kurmanji)", "ku" },
                { "Kyrgyz", "ky" },
                { "Lao", "lo" },
                { "Latin", "la" },
                { "Latvian", "lv" },
                { "Lithuanian", "lt" },
                { "Luxembourgish", "lb" },
                { "Macedonian", "mk" },
                { "Malagasy", "mg" },
                { "Malay", "ms" },
                { "Malayalam", "ml" },
                { "Maltese", "mt" },
                { "Maori", "mi" },
                { "Marathi", "mr" },
                { "Mongolian", "mn" },
                { "Myanmar (Burmese)", "my" },
                { "Nepali", "ne" },
                { "Norwegian", "no" },
                { "Odia", "or" },
                { "Pashto", "ps" },
                { "Persian", "fa" },
                { "Polish", "pl" },
                { "Portuguese", "pt" },
                { "Punjabi", "pa" },
                { "Romanian", "ro" },
                { "Russian", "ru" },
                { "Samoan", "sm" },
                { "Scots Gaelic", "gd" },
                { "Serbian", "sr" },
                { "Sesotho", "st" },
                { "Shona", "sn" },
                { "Sindhi", "sd" },
                { "Sinhala", "si" },
                { "Slovak", "sk" },
                { "Slovenian", "sl" },
                { "Somali", "so" },
                { "Spanish", "es" },
                { "Sundanese", "su" },
                { "Swahili", "sw" },
                { "Swedish", "sv" },
                { "Tajik", "tg" },
                { "Tamil", "ta" },
                { "Telugu", "te" },
                { "Thai", "th" },
                { "Turkish", "tr" },
                { "Ukrainian", "uk" },
                { "Urdu", "ur" },
                { "Uyghur", "ug" },
                { "Uzbek", "uz" },
                { "Vietnamese", "vi" },
                { "Welsh", "cy" },
                { "Xhosa", "xh" },
                { "Yiddish", "yi" },
                { "Yoruba", "yo" },
                { "Zulu", "zu" }
            };

            var selectedItem = (ComboBoxItem)LanguageSelection.SelectedItem;
            var selectedLanguage = selectedItem?.Content?.ToString();

            if (languageCodes.TryGetValue(selectedLanguage, out var languageCode))
                return languageCode;

            return string.Empty;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            httpClient.Dispose();
        }
    }
}