using Messenger.App.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Messenger.App
{
    public partial class ActivationForm : Form
    {
        private readonly string _host = "http://netmessenger.somee.com";

        public ActivationForm()
        {
            InitializeComponent();
            textBox1.MaxLength = 4;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        public static User User { get; set; }

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
                try
                {
                    var httpClient = new HttpClient();

                    var request = new User()
                    {
                        Username = User.Username,
                        EmailCode = Convert.ToInt32(textBox1.Text)
                    };

                    var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync($"{_host}/api/v1/User/ActivateAccount", content);

                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
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

                    var responseString = await response.Content.ReadAsStringAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + ex);
                }
            }
        }
    }
}
