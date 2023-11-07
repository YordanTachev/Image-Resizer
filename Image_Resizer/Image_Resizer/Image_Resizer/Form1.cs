using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Image_Resizer
{
    public partial class Form1 : Form
    {
        Bitmap orgImage;
        Bitmap resizedImage;
        int sF;

        public void ThreadProc()
        {
            int newWidth = orgImage.Width / sF;
            int newHeight = orgImage.Height / sF;
            resizedImage = new Bitmap(newWidth, newHeight);

            for (int x = 0; x < newWidth; x++)
            {
                for (int y = 0; y < newHeight; y++)
                {
                    int totalRed = 0;
                    int totalGreen = 0;
                    int totalBlue = 0;

                    for (int i = 0; i < sF; i++)
                    {
                        for (int j = 0; j < sF; j++)
                        {
                            int offsetX = x * sF + i;
                            int offsetY = y * sF + j;

                            Color pixelColor = orgImage.GetPixel(offsetX, offsetY);

                            totalRed += pixelColor.R;
                            totalGreen += pixelColor.G;
                            totalBlue += pixelColor.B;
                        }
                    }

                    int averageRed = totalRed / (sF * sF);
                    int averageGreen = totalGreen / (sF * sF);
                    int averageBlue = totalBlue / (sF * sF);

                    Color averageColor = Color.FromArgb(averageRed, averageGreen, averageBlue);

                    resizedImage.SetPixel(x, y, averageColor);
                }
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "images | *.png;*.jpg;*.jpeg;*.gif";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtSelect.Text = ofd.FileName;
                orgImage = new Bitmap(ofd.FileName);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
                txtSave.Text = fbd.SelectedPath;
        }

        private void btnResize_Click(object sender, EventArgs e)
        {
            if (orgImage != null && !string.IsNullOrEmpty(textBox1.Text))
            {
                sF = int.Parse(textBox1.Text);
                Thread thread = new Thread(new ThreadStart(ThreadProc));
                thread.Start();
                thread.Join();
                MessageBox.Show("Image Resized");
            }
            else
            {
                MessageBox.Show("Please select the image and provide the resize value first.");
            }
        }

        private void btnFSave_Click(object sender, EventArgs e)
        {
            if (resizedImage != null && !string.IsNullOrEmpty(txtSave.Text))
            {
                resizedImage.Save(txtSave.Text + "\\rImage.png");
                MessageBox.Show("Image Saved");
            }
            else
            {
                MessageBox.Show("Please resize the image and provide the save path first.");
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
