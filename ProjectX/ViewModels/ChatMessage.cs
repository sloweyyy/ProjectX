using System.Windows;
using System.Windows.Input;
using ProjectX.ViewModels;

namespace ProjectX.Views
{
    public class ChatMessage
    {
        public string Message { get; set; }
        public string Sender { get; set; }
        public ICommand CopyCommand { get; set; }

        public ChatMessage()
        {
            CopyCommand = new RelayCommand(CopyToClipboard);
        }

        private void CopyToClipboard(object parameter)
        {
            Clipboard.SetText(Message);
        }


    }
}