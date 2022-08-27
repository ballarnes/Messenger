using Messenger.App.Services;
using System.Text;
using System.Text.Json;
using Flurl.Http;
using Messenger.App.Models;
using Messenger.App.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.App
{
    public partial class MainForm : Form
    {
        private bool _usernameIsValid;
        private bool _passwordIsValid;
        private readonly IHttpService _httpService;

        public MainForm(IHttpService httpService)
        {
            InitializeComponent();
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            failedLabel.Visible = false;
            loadingPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            loadingPictureBox.Image = Image.FromFile(@"Images\loading_gif.gif");
            loadingPictureBox.Visible = false;
            _httpService = httpService;
        }

        private void label4_Enter(object sender, EventArgs e)
        {
            label4.BackColor = Color.White;
            label4.ForeColor = Color.White;
        }

        private void label4_Click(object sender, EventArgs e)
        {
            var registrationForm = Program.ServiceProvider.GetRequiredService<RegistrationForm>();
            registrationForm.Show();
        }

        private async void loginButton_Click(object sender, EventArgs e)
        {
            loadingPictureBox.Visible = true;

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
                failedLabel.Visible = false;

                var result = await _httpService.Login(loginTextBox.Text, passwordTextBox.Text);

                if (result != null)
                {
                    if (!result.isActivated)
                    {
                        var activationForm = Program.ServiceProvider.GetRequiredService<ActivationForm>();
                        ActivationForm.User = result;
                        activationForm.Show();
                    }
                    else
                    {
                        var messengerForm = Program.ServiceProvider.GetRequiredService<MessengerForm>();
                        MessengerForm.User = result;
                        messengerForm.Show();
                        this.Hide();
                    }
                }
                else
                {
                    failedLabel.Visible = true;
                }
            }

            loadingPictureBox.Visible = false;
        }
    }
}