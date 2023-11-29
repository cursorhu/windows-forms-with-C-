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

            //
            //Register shared menu event handler for Image size mode.  
            //comment manually written code here and using [window designer GUI]'s generic event hander registry way is better. 
            //
            //this.streToolStripMenuItem.Click += new System.EventHandler(this.imageToolStripMenuItem_ChildClick);
            //this.actualSizeToolStripMenuItem.Click += new System.EventHandler(this.imageToolStripMenuItem_ChildClick);
            //this.zoomToolStripMenuItem.Click += new System.EventHandler(this.imageToolStripMenuItem_ChildClick);
            //this.autoSizeToolStripMenuItem.Click += new System.EventHandler(this.imageToolStripMenuItem_ChildClick);

            DefineContextMenu();
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //the Close method ensures that all nonmemory resources associated with a form are disposed
            this.Close();
        }

        private void imageToolStripMenuItem_ChildClick(object sender, EventArgs e)
        {
            //Verify sender is type of MenuItem object
            if (sender is MenuItem)
            {
                //The Index property is not available in the object class, 
                //so we need to convert our variable of type object into a variable of type MenuItem (downcast)
                MenuItem mi = (MenuItem)sender;
                _selectedImageMode = mi.Index;
                //set property SizeMode based on selected index.
                pboxPhoto.SizeMode = modeMenuArray[_selectedImageMode];
                //Invalidate the PictureBox control to redisplay the image.
                pboxPhoto.Invalidate();
            }
        }

        private void imageToolStripMenuItem_Popup(object sender, EventArgs e)
        {
            //Menu variable here could also be defined as a MenuItem object
            if (sender is Menu)
            {
                //Determine if an image is loaded
                bool bImageloaded = (pboxPhoto.Image != null);
                Menu m = (Menu)sender;
                //Iterate over each sub-menu item
                foreach (MenuItem mi in m.MenuItems)
                {
                    mi.Enabled = bImageloaded;
                    mi.Checked = (this._selectedImageMode == mi.Index);
                }
            }
        }

        private void DefineContextMenu()
        {
            // Copy the View menu into ctxtMenuView
            //ToolStripMenuItem ctxsmi = new ToolStripMenuItem("View", null, this.viewToolStripMenuItem);
            ToolStripMenuItem ctxsmi = this.viewToolStripMenuItem;

            ctxMenuStripView.Items.AddRange(ctxsmi.DropDownItems);
        }
    }
}
