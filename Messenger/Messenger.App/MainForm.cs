using Messenger.App.Services;
using System.Text;
using System.Text.Json;
using Flurl.Http;
using Messenger.App.Models;

namespace Messenger.App
{
    public partial class MainForm : Form
    {
        private bool _usernameIsValid;
        private bool _passwordIsValid;
        private readonly string _host = "http://netmessenger.somee.com";

        public MainForm()
        {
            InitializeComponent();
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            failedLabel.Visible = false;
        }

        private void label4_Enter(object sender, EventArgs e)
        {
            label4.BackColor = Color.White;
            label4.ForeColor = Color.White;
        }

        private void label4_Click(object sender, EventArgs e)
        {
            var registrationForm = new RegistrationForm();
            registrationForm.Show();
        }

        private async void loginButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(loginTextBox.Text))
            {
                _usernameIsValid = false;
                pictureBox1.Image = Image.FromFile(@"Images\error_icon.png");
            }
            else
            {
                _usernameIsValid = true;
            }

            if (String.IsNullOrEmpty(passwordTextBox.Text))
            {
                _passwordIsValid = false;
                pictureBox2.Image = Image.FromFile(@"Images\error_icon.png");
            }
            else
            {
                _passwordIsValid = true;
            }

            if (_usernameIsValid && _passwordIsValid)
            {
                try
                {
                    failedLabel.Visible = false;
                    var response = await $"{_host}/api/v1/User/Login"
                        .PostJsonAsync(new { Username = loginTextBox.Text, Password = EncodingService.GetHashString(passwordTextBox.Text) })
                        .ReceiveJson<User>();

                    if (!response.isActivated)
                    {
                        var activationForm = new ActivationForm();
                        ActivationForm.User = response;
                        activationForm.Show();
                    }
                    else
                    {
                        var messengerForm = new MessengerForm();

                        MessengerForm.User = response;

                        messengerForm.Show();
                        this.Hide();
                    }
                }
                catch (Exception ex)
                {
                    failedLabel.Visible = true;
                    Console.WriteLine(ex.Message + ex);
                }
            }
        }
    }
}