using Flurl.Http;
using Messenger.App.Models;
using Messenger.App.Services;
using Messenger.App.Services.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;

namespace Messenger.App
{
    public partial class MessengerForm : Form
    {
        private User _userChatWith = new User();
        private List<User> _foundUsers = new List<User>();
        private readonly IHttpService _httpService;

        public MessengerForm(IHttpService httpService)
        {
            InitializeComponent();
            HubConnection = new HubConnectionBuilder()
                .WithUrl($"http://localhost:5000/notification")
                .WithAutomaticReconnect()
                .Build();

            _httpService = httpService;

            HubConnection.On<Models.Message>("Receive", message => ReceiveMessage(message));
            
            usersTextBox.Enabled = false;
            messageTextBox.Enabled = false;
            sendButton.Enabled = false;
            loadingPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            loadingPictureBox.Image = Image.FromFile(@"Images\loading_gif.gif");
            loadingPictureBox.Visible = false;

            userPhotoPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            userPhotoPictureBox.Image = Image.FromFile(@"Images\user_photo.png");

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;

            Initialize();
        }

        public static User User { get; set; } = null!;

        public static HubConnection HubConnection { get; set; } = null!;

        public async void Initialize()
        {
            await HubConnection.StartAsync();

            if (HubConnection.ConnectionId != null)
            {
                var connectedUser = new ConnectedUser()
                {
                    Id = User.Id,
                    Username = User.Username,
                    ConnectionId = HubConnection.ConnectionId
                };

                await HubConnection.SendAsync("ConnectUser", connectedUser);
            }

            label5.Text = User.Username;
        }

        private async void sendButton_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(messageTextBox.Text))
            {
                var message = new Models.Message()
                {
                    Text = EncodingService.Encrypt(messageTextBox.Text),
                    Date = DateTime.Now,
                    From = EncodingService.Encrypt(User.Username),
                    To = EncodingService.Encrypt(_userChatWith.Username)
                };

                chatTextBox.AppendText($"{message.Date} {User.Username} -> {_userChatWith.Username}: {messageTextBox.Text}\n");
                await HubConnection.SendAsync("SendMessage", message);
                messageTextBox.Clear();
            }
        }

        private async void MessengerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (HubConnection.ConnectionId != null)
            {
                var connectedUser = new ConnectedUser()
                {
                    Id = User.Id,
                    Username = User.Username,
                    ConnectionId = HubConnection.ConnectionId
                };

                await HubConnection.SendAsync("DisconnectUser", connectedUser);
            }

            Application.Exit();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(searchUserTextBox.Text))
            {
                loadingPictureBox.Visible = true;

                var result = await _httpService.GetUsers(searchUserTextBox.Text);

                usersTextBox.Items.Clear();
                usersTextBox.Enabled = true;
                _foundUsers.Clear();

                if (result != null)
                {
                    foreach (var user in result)
                    {
                        if (user.Username != User.Username)
                        {
                            usersTextBox.Items.Add($"@{user.Username} | {user.Name} {user.Surname}");
                            _foundUsers.Add(user);
                        }
                    }
                }

                loadingPictureBox.Visible = false;
            }
        }

        private async void usersTextBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _userChatWith = _foundUsers[usersTextBox.SelectedIndex];
            label3.Text = $"{_userChatWith.Name} {_userChatWith.Surname} ({_userChatWith.Username})";
            chatTextBox.Enabled = true;
            chatTextBox.Clear();
            messageTextBox.Enabled = true;
            sendButton.Enabled = true;
            await LoadMessages(EncodingService.Encrypt(User.Username), EncodingService.Encrypt(_userChatWith.Username));
        }

        private async void ReceiveMessage(Models.Message message)
        {
            var decryptedMessage = EncodingService.Decrypt(message.Text);
            var decryptedFrom = EncodingService.Decrypt(message.From);
            var decryptedTo = EncodingService.Decrypt(message.To);

            if (message.From == _userChatWith.Username)
            {
                chatTextBox.AppendText($"{message.Date} {decryptedFrom} -> {decryptedTo}: {decryptedMessage}\n");
            }
            else
            {
                if (MessageBox.Show(
                    $"You have a new message from {message.From}. Would you like to open chat?",
                    "New message!",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly) == DialogResult.Yes)
                {
                    var result = await _httpService.GetUsers(message.From);

                    if (result != null)
                    {
                        var user = result.FirstOrDefault();

                        chatTextBox.Clear();
                        _userChatWith = user!;
                        label3.Text = user!.Username;
                        messageTextBox.Enabled = true;
                        sendButton.Enabled = true;
                        await LoadMessages(EncodingService.Encrypt(User.Username), EncodingService.Encrypt(_userChatWith.Username));
                    }
                }
            }
        }

        private async Task LoadMessages(string firstUsername, string secondUsername)
        {
            loadingPictureBox.Visible = true;

            var result = await _httpService.GetMessages(firstUsername, secondUsername);

            if (result != null)
            {
                foreach (var message in result)
                {
                    var decryptedMessage = EncodingService.Decrypt(message.Text);
                    var decryptedFrom = EncodingService.Decrypt(message.From);
                    var decryptedTo = EncodingService.Decrypt(message.To);

                    chatTextBox.AppendText($"{message.Date} {decryptedFrom} -> {decryptedTo}: {decryptedMessage} \n");
                }
            }

            loadingPictureBox.Visible = false;
        }

        private void chatTextBox_TextChanged(object sender, EventArgs e)
        {
            chatTextBox.SelectionStart = chatTextBox.Text.Length;
            chatTextBox.ScrollToCaret();
        }

        private void messageTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                sendButton_Click(sender, e);
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
            {
                textBox1.Text = User.Name;
                textBox2.Text = User.Surname;
                textBox3.Text = User.Username;
                textBox4.Text = User.Email;
                submitInfoButton.Visible = false;
                label10.Visible = false;
            }
            else
            {
                changeInfoButton.Enabled = true;
                textBox1.ReadOnly = true;
                textBox2.ReadOnly = true;
            }
        }

        private void changeInfoButton_Click(object sender, EventArgs e)
        {
            changeInfoButton.Enabled = false;
            submitInfoButton.Visible = true;
            textBox1.ReadOnly = false;
            textBox2.ReadOnly = false;
        }

        private async void submitInfoButton_Click(object sender, EventArgs e)
        {
            var nameIsValid = false;
            var surnameIsValid = false;

            if (String.IsNullOrEmpty(textBox1.Text))
            {
                pictureBox1.Image = Image.FromFile(@"Images\error_icon.png");
            }
            else
            {
                nameIsValid = true;
                pictureBox1.Image = Image.FromFile(@"Images\success_icon.png");
            }

            if (String.IsNullOrEmpty(textBox2.Text))
            {
                pictureBox2.Image = Image.FromFile(@"Images\error_icon.png");
            }
            else
            {
                surnameIsValid = true;
                pictureBox2.Image = Image.FromFile(@"Images\success_icon.png");
            }

            if (nameIsValid && surnameIsValid)
            {
                var result = await _httpService.UpdateUserInfo(User.Id, textBox1.Text, textBox2.Text, User.Email, User.Username);
                
                if (result != null)
                {
                    if (result.IsSuccessStatusCode)
                    {
                        changeInfoButton.Enabled = true;
                        submitInfoButton.Visible = false;
                        textBox1.ReadOnly = true;
                        textBox2.ReadOnly = true;
                        label10.Visible = true;

                        User.Name = textBox1.Text;
                        User.Surname = textBox2.Text;
                    }
                    else
                    {
                        label10.ForeColor = Color.Red;
                        label10.Text = "Error!";
                        label10.Visible = true;
                    }
                }
            }
        }
    }
}
