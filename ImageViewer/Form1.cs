using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageViewer
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }
    private void Form1_Load(object sender, EventArgs e)
    {
      Byte[] test = new Byte[2];
      // Construct an image object from a file in the local directory.
      // ... This file must exist in the solution.
      Image image = Image.FromStream(new MemoryStream(test));
      // Set the PictureBox image property to this image.
      // ... Then, adjust its height and width properties.
      PictureBox pic = new PictureBox();
      pic.Image = image;
      pic.Height = image.Height;
      pic.Width = image.Width;

    }
    private void Images(object sender, EventArgs e)
    {
      PictureBox pb1 = new PictureBox();
      pb1.Image = Image.FromFile("../../example.png");
      pb1.Location = new Point(100, 100);
      pb1.Size = new Size(500, 500);
      this.Controls.Add(pb1);
    }
  }
}
