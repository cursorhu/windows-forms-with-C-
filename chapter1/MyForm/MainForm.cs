using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyForm
{
    public partial class ImageLoader : Form
    {
        public ImageLoader()
        {
            InitializeComponent();

            //
            //Show the Version in properties to window GUI
            //Do not add the code to InitializeComponent because it's auto generated and may be overwritten
            //
            Version ver = new Version(Application.ProductVersion);
            this.Text = String.Format("ImageLoader {0}.{1}.{2}.{3}", ver.Major, ver.Minor, ver.Build, ver.Revision);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            //文件Dialog
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Open Photo";
            dlg.Filter = "jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";
            if(dlg.ShowDialog() == DialogResult.OK)
            {
                //avoid program broken when input is not a picture.
                try
                {
                    pboxPhoto.Image = new Bitmap(dlg.OpenFile());
                }
                catch (Exception ex)
                {
                    // Handle exception
                    MessageBox.Show("Unable to load file: " + ex.Message);
                }
            }

            //对于文件，内存等资源还是建议用完立即释放，调用MainForm.Designer重写的Dispose
            dlg.Dispose();
        }
  
    }
}
