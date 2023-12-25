using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Newtonsoft.Json;

namespace ProjectX.Views
{
    public partial class Gemini : Window
    {
        private readonly HttpClient client;
        private ObservableCollection<ChatMessage> chatMessages;


        public Gemini()
        {
            InitializeComponent();
            client = new HttpClient();
            UserInputTextBox.KeyDown += UserInputTextBox_KeyDown;
            chatMessages = new ObservableCollection<ChatMessage>();
            ChatListBox.ItemsSource = chatMessages;

        }

        private void UserInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Prevent the Enter key from being processed by the TextBox
                e.Handled = true;

                // Call the SendMessage_Click method
                SendMessage_Click(sender, e);

                // Empty the textbox
                UserInputTextBox.Clear();
            }
        }

        private async void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get user input
                string userInput = UserInputTextBox.Text;

                // Add user's message to the chat
                AddMessage(userInput, "User");

                // Get bot's response using the API
                string botResponse = await GetBotResponse(userInput);

                // Add bot's response to the chat
                AddMessage(botResponse, "Bot");

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                // Clear user input in a finally block to ensure it always happens
                UserInputTextBox.Clear();
            }
        }


        private async Task<string> GetBotResponse(string userInput)
        {
            try
            {
                string apiEndpoint = "https://projectx-landingpage-production.up.railway.app/gemini";

                string jsonRequest = $"{{ \"question\": \"{userInput}\" }}";

                using (var request = new HttpRequestMessage(HttpMethod.Post, apiEndpoint))
                {
                    request.Content = new StringContent(jsonRequest);
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    using (var response = await client.SendAsync(request))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string jsonResponse = await response.Content.ReadAsStringAsync();
                            var responseObject = JsonConvert.DeserializeObject<BotResponse>(jsonResponse);

                            return responseObject?.response?.Replace("*", string.Empty);
                        }

                        // Log API errors to the Output window
                        LogApiError(response);

                        return "API Error";
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                // Log HTTP request errors to the Output window
                LogHttpRequestError(ex);

                return "HTTP Request Error";
            }
            catch (Exception ex)
            {
                // Log other errors to the Output window
                Debug.WriteLine($"Error: {ex.Message}");
                return "Error";
            }
        }

        private void LogApiError(HttpResponseMessage response)
        {
            Debug.WriteLine($"API Error: {response.StatusCode} - {response.ReasonPhrase}");
            string responseBody = response.Content.ReadAsStringAsync().Result;
            Debug.WriteLine($"Response Body: {responseBody}");
        }

        private void LogHttpRequestError(HttpRequestException ex)
        {
            Debug.WriteLine($"HTTP Request Error: {ex.Message}");
            if (ex.InnerException != null)
            {
                Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
        }

        private void AddMessage(string message, string sender)
        {
            chatMessages.Add(new ChatMessage { Message = message, Sender = sender });

            // Simplify scrolling to the bottom of the ListBox
            ChatListBox.ScrollIntoView(chatMessages[chatMessages.Count - 1]);
        }


        private void ChatListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
