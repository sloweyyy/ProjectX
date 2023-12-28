using System;
using MongoDB.Driver;
using System.Windows.Controls;
using System.Windows;
using RestSharp;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Linq;

namespace ProjectX.Views
{
    public partial class LoginWindow : Window
    {
        private readonly IMongoCollection<User> _usersCollection;
        private string currentVersion = File.ReadAllText("..\\..\\version.txt");

        public LoginWindow()
        {
            try
            {
                InitializeComponent();
                _usersCollection = GetMongoCollection();
                Loaded += Window_Loaded;
                PasswordBox.KeyDown += PasswordBox_KeyDown;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        private IMongoCollection<User> GetMongoCollection()
        {
            string connectionString = "mongodb+srv://slowey:tlvptlvp@projectx.3vv2dfv.mongodb.net/";
            string databaseName = "ProjectX";

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<User>("users");
        }

        private User GetUser(string username)
        {
            var filter = Builders<User>.Filter.Eq(u => u.username, username);
            return _usersCollection.Find(filter).FirstOrDefault();
        }


        private void CheckLogin_Click(object sender, RoutedEventArgs e)
        {
            var selectedUsername = UsernameTextBox.Text;
            var enteredPassword = PasswordBox.Password;

            if (selectedUsername != null && !string.IsNullOrEmpty(enteredPassword))
            {
                var user = GetUser(selectedUsername);

                if (user == null)
                {
                    MessageBox.Show("Sai tên tài khoản hoặc mật khẩu.");
                    return;
                }

                if (VerifyPassword(enteredPassword, user.password))
                {
                    UpdateLastUsed(selectedUsername);
                    MainWindow mainWindow = new MainWindow(selectedUsername);
                    mainWindow.Show();

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Sai tên tài khoản hoặc mật khẩu.");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
            }
        }

        private void UpdateLastUsed(string username)
        {
            var filter = Builders<User>.Filter.Eq(u => u.username, username);
            var update = Builders<User>.Update.Set(u => u.last_used_at, DateTime.Now);

            _usersCollection.UpdateOne(filter, update);
        }




        private bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedPasswordHash);
        }


        private void OpenRegisterForm_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.Show();

            this.Close();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            var client = new RestClient("https://raw.githubusercontent.com/sloweyyy/IT008.O12/main/ProjectX/version.txt");

            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            if (!response.Content.Contains(currentVersion))
            {
                MessageBox.Show("Đã có phiên bản mới. Hãy cập nhật nhé!");
                System.Diagnostics.Process.Start("https://github.com/sloweyyy/IT008.O12/releases");
            }



        }

        private void PasswordBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                CheckLogin_Click(sender, e);
            }
        }

        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            UsernameRecoveryLabel.Visibility = Visibility.Visible;
            UsernameRecoveryTextBox.Visibility = Visibility.Visible;
            EmailRecoveryLabel.Visibility = Visibility.Visible;
            EmailRecoveryTextBox.Visibility = Visibility.Visible;
            ConfirmRecoveryButton.Visibility = Visibility.Visible;

        }

        private void ConfirmRecovery_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameRecoveryTextBox.Text;
            string userEmail = EmailRecoveryTextBox.Text;

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(userEmail))
            {
                var user = GetUserByUsernameAndEmail(username, userEmail);

                if (user != null)
                {
                    string newPassword = GenerateRandomPassword(10);
                    UpdatePassword(user.username, newPassword);
                    SendPasswordRecoveryEmail(userEmail, newPassword);

                    MessageBox.Show("Mật khẩu mới đã được gửi đến địa chỉ email của bạn.");
                }
                else
                {
                    MessageBox.Show("Thông tin không khớp hoặc không tồn tại trong hệ thống.");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
            }
        }

        private User GetUserByUsernameAndEmail(string username, string email)
        {
            var filter = Builders<User>.Filter.And(
                Builders<User>.Filter.Eq(u => u.username, username),
                Builders<User>.Filter.Eq(u => u.email, email)
            );

            return _usersCollection.Find(filter).FirstOrDefault();
        }


        private void UpdatePassword(string username, string newPassword)
        {
            var filter = Builders<User>.Filter.Eq(u => u.username, username);
            var update = Builders<User>.Update.Set(u => u.password, BCrypt.Net.BCrypt.HashPassword(newPassword));

            _usersCollection.UpdateOne(filter, update);
        }

        private string GenerateRandomPassword(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void SendPasswordRecoveryEmail(string userEmail, string newPassword)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("sloweycontact@gmail.com", "qpxb dmnf wnrk ttyt\r\n"), // Use your App Password here
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("sloweycontact@gmail.com"),
                Subject = "Khôi phục mật khẩu",
                IsBodyHtml = true, // Set to true for HTML content
            };

            // Customize the email body with HTML formatting
            string htmlBody = $@"
        <html>
            <body>
                <p>Xin chào {userEmail},</p>
                <p>Chúng tôi nhận được yêu cầu khôi phục mật khẩu cho tài khoản của bạn.</p>
                <p>Mật khẩu mới của bạn là: <strong>{newPassword}</strong></p>
                <p>Đừng chia sẻ mật khẩu này với người khác. Để đảm bảo an toàn, bạn nên đổi mật khẩu ngay sau khi đăng nhập.</p>
                <p>Trân trọng,</p>
                <p>Đội ngũ hỗ trợ ProjectX</p>
            </body>
        </html>";

            mailMessage.Body = htmlBody;
            mailMessage.To.Add(userEmail);

            smtpClient.Send(mailMessage);
        }
    }
}