using Flurl.Http;
using Messenger.App.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Messenger.App
{
    public partial class MessengerForm : Form
    {
        private User _userChatWith = new User();
        private List<User> _foundUsers = new List<User>();
        private readonly string _host = "http://netmessenger.somee.com";

        public MessengerForm()
        {
            InitializeComponent();
            HubConnection = new HubConnectionBuilder()
                .WithUrl($"{_host}/notification")
                .WithAutomaticReconnect()
                .Build();

            HubConnection.On<Models.Message>("Receive", message => ReceiveMessage(message));
            
            usersTextBox.Enabled = false;
            messageTextBox.Enabled = false;
            sendButton.Enabled = false;
            loadingPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            loadingPictureBox.Image = Image.FromFile(@"Images\loading_gif.gif");
            loadingPictureBox.Visible = false;

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
                    Text = messageTextBox.Text,
                    Date = DateTime.Now,
                    From = User.Username,
                    To = _userChatWith.Username
                };

                chatTextBox.AppendText($"{message.Date} {message.From} -> {message.To}: {message.Text}\n");
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

                try
                {
                    var response = await $"{_host}/api/v1/User/GetUsers"
                        .PostJsonAsync(new { Username = searchUserTextBox.Text })
                        .ReceiveJson<List<User>>();

                    usersTextBox.Items.Clear();
                    usersTextBox.Enabled = true;
                    _foundUsers.Clear();

                    foreach (var user in response)
                    {
                        if (user.Username != User.Username)
                        {
                            usersTextBox.Items.Add($"@{user.Username} | {user.Name} {user.Surname}");
                            _foundUsers.Add(user);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + ex);
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
            await LoadMessages(User.Username, _userChatWith.Username);
        }

        private async void ReceiveMessage(Models.Message message)
        {
            if (message.From == _userChatWith.Username)
            {
                chatTextBox.AppendText($"{message.Date} {message.From} -> {message.To}: {message.Text}\n");
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
                    try
                    {
                        var response = await $"{_host}/api/v1/User/GetUsers"
                            .PostJsonAsync(new { Username = message.From })
                            .ReceiveJson<List<User>>();

                        var user = response.FirstOrDefault();

                        if (user != null)
                        {
                            chatTextBox.Clear();
                            _userChatWith = user;
                            label3.Text = user.Username;
                            messageTextBox.Enabled = true;
                            sendButton.Enabled = true;
                            await LoadMessages(User.Username, _userChatWith.Username);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + ex);
                    }
                }
            }
        }

        private async Task LoadMessages(string firstUsername, string secondUsername)
        {
            loadingPictureBox.Visible = true;

            try
            {
                var response = await $"{_host}/api/v1/Message/GetMessages"
                    .PostJsonAsync(new { FirstUsername = firstUsername, SecondUsername = secondUsername })
                    .ReceiveJson<List<Models.Message>>();

                var messages = response;

                if (messages != null)
                {
                    foreach (var message in messages)
                    {
                        chatTextBox.AppendText($"{message.Date} {message.From} -> {message.To}: {message.Text}\n");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex);
            }

            loadingPictureBox.Visible = false;
        }

        private void chatTextBox_TextChanged(object sender, EventArgs e)
        {
            chatTextBox.SelectionStart = chatTextBox.Text.Length;
            chatTextBox.ScrollToCaret();
        }
    }
}
