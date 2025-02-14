using System;
using System.Drawing;
using System.Windows.Forms;

namespace MinecraftModUpdater
{
    public partial class CustomMessageBox : Form
    {
        public CustomMessageBox(string message, string title)
        {
            InitializeComponent();
            lblMessage.Text = message;
            lblTitle.Text = title;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CustomMessageBox_Paint(object sender, PaintEventArgs e)
        {
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(30, 30, 30)))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }
    }
}
