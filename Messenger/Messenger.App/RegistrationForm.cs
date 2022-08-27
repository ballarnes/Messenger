using Messenger.App.Models;
using Messenger.App.Models.Requests;
using Messenger.App.Services;
using Messenger.App.Services.Interfaces;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Messenger.App
{
    public partial class RegistrationForm : Form
    {
        private bool _nameIsValid;
        private bool _surnameIsValid;
        private bool _emailIsValid;
        private bool _usernameIsValid;
        private bool _passwordIsValid;
        private readonly IHttpService _httpService;

        public RegistrationForm(IHttpService httpService)
        {
            InitializeComponent();
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox6.SizeMode = PictureBoxSizeMode.StretchImage;
            textBox6.MaxLength = 4;
            textBox6.Visible = false;
            label6.Visible = false;
            button2.Visible = false;
            loadingPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            loadingPictureBox.Image = Image.FromFile(@"Images\loading_gif.gif");
            loadingPictureBox.Visible = false;
            _httpService = httpService;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            loadingPictureBox.Visible = true;

            if (String.IsNullOrEmpty(textBox1.Text))
            {
                pictureBox4.Image = Image.FromFile(@"Images\error_icon.png");
            }
            else
            {
                _nameIsValid = true;
                pictureBox4.Image = Image.FromFile(@"Images\success_icon.png");
            }

            if (String.IsNullOrEmpty(textBox2.Text))
            {
                pictureBox3.Image = Image.FromFile(@"Images\error_icon.png");
            }
            else
            {
                _surnameIsValid = true;
                pictureBox3.Image = Image.FromFile(@"Images\success_icon.png");
            }

            if (String.IsNullOrEmpty(textBox4.Text))
            {
                pictureBox1.Image = Image.FromFile(@"Images\error_icon.png");
            }
            else
            {
                await CheckUsername(textBox4.Text);
            }

            if (String.IsNullOrEmpty(textBox5.Text))
            {
                pictureBox5.Image = Image.FromFile(@"Images\error_icon.png");
            }
            else
            {
                _passwordIsValid = true;
                pictureBox5.Image = Image.FromFile(@"Images\success_icon.png");
            }

            if (String.IsNullOrEmpty(textBox3.Text))
            {
                pictureBox2.Image = Image.FromFile(@"Images\error_icon.png");
            }
            else
            {
                var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                var match = regex.Match(textBox3.Text);

                if (!match.Success)
                {
                    pictureBox2.Image = Image.FromFile(@"Images\error_icon.png");
                }
                else
                {
                    _emailIsValid = true;
                    pictureBox2.Image = Image.FromFile(@"Images\success_icon.png");
                }
            }

            if (_nameIsValid && _surnameIsValid && _emailIsValid && _usernameIsValid && _passwordIsValid)
            {
                var result = await _httpService.RegisterUser(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text);
                
                if (result != null)
                {
                    if (result.IsSuccessStatusCode)
                    {
                        textBox1.Enabled = false;
                        textBox2.Enabled = false;
                        textBox3.Enabled = false;
                        textBox4.Enabled = false;
                        textBox5.Enabled = false;
                        pictureBox1.Visible = false;
                        pictureBox2.Visible = false;
                        pictureBox3.Visible = false;
                        pictureBox4.Visible = false;
                        pictureBox5.Visible = false;
                        button1.Enabled = false;
                        textBox6.Visible = true;
                        label6.Visible = true;
                        button2.Visible = true;
                    }
                }
            }

            loadingPictureBox.Visible = false;
        }

        private async void textBox4_TextChanged(object sender, EventArgs e)
        {
            await CheckUsername(textBox4.Text);
        }

        private async Task CheckUsername(string username)
        {
            var result = await _httpService.CheckUsername(username);

            if (result != null)
            {
                if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _usernameIsValid = true;
                    pictureBox1.Image = Image.FromFile(@"Images\success_icon.png");
                }
                else
                {
                    pictureBox1.Image = Image.FromFile(@"Images\error_icon.png");
                }
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox6.Text))
            {
                pictureBox6.Image = Image.FromFile(@"Images\error_icon.png");
            }
            else
            {
                var result = await _httpService.ActivateAccout(textBox4.Text, textBox6.Text);

                if (result != null)
                {
                    if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        pictureBox6.Image = Image.FromFile(@"Images\error_icon.png");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(
                        "Now you can login",
                        "Success!",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);
                        pictureBox6.Image = Image.FromFile(@"Images\success_icon.png");
                        this.Close();
                    }
                }
            }
        }
    }
}
