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
        /// <summary>
        /// Mode settings for the View->Image submenu.
        /// The order here must correspond to the order
        /// of menus in the submenu.
        /// </summary>
        private PictureBoxSizeMode[] modeMenuArray =
        {
            PictureBoxSizeMode.Normal,
            PictureBoxSizeMode.AutoSize,
            PictureBoxSizeMode.StretchImage,
            PictureBoxSizeMode.Zoom,
        };
        //hold the currently selected display mode for the image
        private int _selectedImageMode = 0;


        public ImageLoader()
        {
            InitializeComponent();

            //ALL manually added code must add after InitializeComponent! 
            //Do not add the code inside InitializeComponent because it is auto generated and your edit may be overwritten!

            //
            //Show the Version in properties to window GUI
            //
            Version ver = new Version(Application.ProductVersion);
            this.Text = String.Format("ImageLoader {0}.{1}.{2}.{3}", ver.Major, ver.Minor, ver.Build, ver.Revision);

        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //文件Dialog
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Open Photo";
            dlg.Filter = "jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                //avoid program broken when input is not a picture.
                try
                {
                    toolStripStatusLabelFile.Text = "Loading " + dlg.FileName;
                    pboxPhoto.Image = new Bitmap(dlg.OpenFile());

                    //initialize the status bar
                    toolStripStatusLabelFile.Text = dlg.FileName;
                    toolStripStatusLabelSize.Text = String.Format("Image Size: {0}x{1}", pboxPhoto.Image.Width, pboxPhoto.Image.Height);
                    toolStripStatusLabelPercent.Text = String.Format("Display Size: 100%"); //default is Normal Mode(Actual Image size).
                }
                catch (Exception ex)
                {
                    toolStripStatusLabelFile.Text = "Fail to load " + dlg.FileName;
                    // Handle exception
                    MessageBox.Show("Unable to load file: " + ex.Message);
                }
            }

            //对于文件，内存等资源还是建议用完立即释放，调用MainForm.Designer重写的Dispose
            dlg.Dispose();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //the Close method ensures that all nonmemory resources associated with a form are disposed
            this.Close();
        }

        private void imageToolStripMenuItem_ChildClick(object sender, EventArgs e)
        {
            //Verify sender is type of ToolStripMenuItem object
            //Use ToolStripMenuItem instead of MenuItem for .net >= 3.1
            if (sender is ToolStripMenuItem)
            {
                //The Index property is not available in the object class, 
                //so we need to convert our variable of type object into a variable of type ToolStripMenuItem (downcast)
                ToolStripMenuItem mi = (ToolStripMenuItem)sender;
                _selectedImageMode = mi.Owner.Items.IndexOf(mi);
                //set property SizeMode based on selected index.
                pboxPhoto.SizeMode = modeMenuArray[_selectedImageMode];
                //Invalidate the PictureBox control to redisplay the image.
                pboxPhoto.Invalidate();

                //update image display state bar
                int cliWidth = pboxPhoto.ClientSize.Width;
                int cliHeight = pboxPhoto.ClientSize.Height;
                int imgWidth = pboxPhoto.Image.Width; //This gives the actual size of the image
                int imgHeight = pboxPhoto.Image.Height; //This gives the actual size of the image
                int percent = 100;
                switch (pboxPhoto.SizeMode)
                {
                    case PictureBoxSizeMode.Normal: //no resize
                    case PictureBoxSizeMode.AutoSize: 
                        percent = 100;
                        break;
                    case PictureBoxSizeMode.StretchImage: //resize factor depends on window
                        percent = 100 * cliWidth * cliHeight / (imgWidth * imgHeight);
                        break;
                    case PictureBoxSizeMode.Zoom: //resize factor depends on hight
                        percent = 100 * cliHeight / imgHeight;
                        break;
                }
                //toolStripStatusLabelSize.Text = String.Format("Image Size: {0}x{1}", imgWidth, imgHeight); //image size is based on .jpg, unchanged.
                toolStripStatusLabelPercent.Text = String.Format("Display Size: {0}%", percent);
                //redraw status bar.
                statusStripMain.Invalidate(); 
            }
        }

        private void imageToolStripMenuItem_Popup(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem)
            {
                //Determine if an image is loaded
                bool bImageloaded = (pboxPhoto.Image != null);
                ToolStripMenuItem m = (ToolStripMenuItem)sender;
                //Iterate over each sub-menu item
                foreach (ToolStripMenuItem mi in m.DropDownItems)
                {
                    mi.Enabled = bImageloaded;
                    mi.Checked = (this._selectedImageMode == mi.Owner.Items.IndexOf(mi));
                }
            }
        }

    }
}
