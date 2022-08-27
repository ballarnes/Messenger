using Messenger.App.Models;
using Messenger.App.Services.Interfaces;

namespace Messenger.App
{
    public partial class ActivationForm : Form
    {
        private readonly IHttpService _httpService;

        public ActivationForm(IHttpService httpService)
        {
            InitializeComponent();
            textBox1.MaxLength = 4;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            _httpService = httpService;
        }

        public static User User { get; set; } = null!;

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox1.Text))
            {
                pictureBox1.Image = Image.FromFile(@"Images\error_icon.png");
            }
            else
            {
                var result = await _httpService.ActivateAccout(User.Username, textBox1.Text);

                if (result != null)
                {
                    if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        pictureBox1.Image = Image.FromFile(@"Images\error_icon.png");
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
                        pictureBox1.Image = Image.FromFile(@"Images\success_icon.png");
                        this.Close();
                    }
                }
            }
        }
    }
}
