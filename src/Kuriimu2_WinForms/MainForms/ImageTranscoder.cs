using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Kanvas;
using Kanvas.Models;

namespace Kuriimu2_WinForms.MainForms
{
    public partial class ImageTranscoder : DynamicImageView
    {
        private bool _imgLoaded;
        private Stream _imgStream;

        public ImageTranscoder()
        {
            InitializeComponent();

            LoadMainComboBoxes(cbEncoding, cbSwizzle);

            EncodingPropertyPanel = splExtendedProperties.Panel1;
            SwizzlePropertyPanel = splExtendedProperties.Panel2;
            UpdateExtendedProperties();

            UpdateForm();
        }

        private void TranscodeImage()
        {
            if (!_imgLoaded || pbSource.Image == null)
                return;

            ToggleProperties(false);
            ToggleForm(false);

            var sourceImage = (Bitmap)Image.FromStream(_imgStream);

            try
            {
                var encoding = CreateColorEncoding(pbSource.Image.Width, pbSource.Image.Height);
                var swizzle = CreateImageSwizzle(pbSource.Image.Width, pbSource.Image.Height);

                var settings = new ImageSettings(encoding, pbSource.Image.Width, pbSource.Image.Height)
                {
                    Swizzle = swizzle
                };
                var imageData = Kolors.Save(sourceImage, settings);
                pbTarget.Image = Kolors.Load(imageData, settings);
            }
            catch (TargetInvocationException tie)
            {
                MessageBox.Show(tie.InnerException?.Message, "Encoding or Type parameters invalid.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Exception catched.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            tslPbHeightSource.Text = pbSource.Image.Height.ToString();
            tslPbWidthSource.Text = pbSource.Image.Width.ToString();
            tslPbHeightTarget.Text = pbTarget.Image.Height.ToString();
            tslPbWidthTarget.Text = pbTarget.Image.Width.ToString();

            ToggleProperties(true);
            ToggleForm(true);
        }

        private void OpenImage(string imgFile)
        {
            _imgStream = File.OpenRead(imgFile);
            pbSource.Image = (Bitmap)Image.FromStream(_imgStream);
            _imgLoaded = true;

            UpdateForm();
        }

        private void ExportImage()
        {
            var sfd = new SaveFileDialog
            {
                Title = "Export image...",
                Filter = "Portable Network Graphics (*.png)|*.png|JPEG (*.jpg)|*.jpg"
            };

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            pbTarget.Image.Save(sfd.FileName);
        }

        private void UpdateForm()
        {
            cbEncoding.Enabled = cbEncoding.Items.Count > 0;
            cbSwizzle.Enabled = cbSwizzle.Items.Count > 0;

            exportToolStripMenuItem.Enabled = _imgLoaded;

            btnTranscode.Enabled = _imgLoaded;
        }

        #region Toggling UI

        private void ToggleProperties(bool toggle)
        {
            TogglePanel(splExtendedProperties.Panel1, toggle);
            TogglePanel(splExtendedProperties.Panel2, toggle);
        }

        private void TogglePanel(Panel panel, bool state)
        {
            var textBoxes = panel.Controls.OfType<TextBox>();
            var comboBoxes = panel.Controls.OfType<ComboBox>();
            var checkBoxes = panel.Controls.OfType<CheckBox>();

            foreach (var textBox in textBoxes)
                textBox.Enabled = state;
            foreach (var comboBox in comboBoxes)
                comboBox.Enabled = state;
            foreach (var checkBox in checkBoxes)
                checkBox.Enabled = state;
        }

        private void ToggleForm(bool toggle)
        {
            cbEncoding.Enabled = toggle;
            cbSwizzle.Enabled = toggle;
            openToolStripMenuItem.Enabled = toggle;
            exportToolStripMenuItem.Enabled = toggle;
            btnTranscode.Enabled = toggle;
        }

        #endregion

        #region Events

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog()
            {
                Title = "Open image...",
                Filter = "Portable Network Graphics (*.png)|*.png|JPEG (*.jpg)|*.jpg"
            };

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            OpenImage(ofd.FileName);
            TranscodeImage();
        }

        private void ExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportImage();
        }

        private void BtnTranscode_Click(object sender, EventArgs e)
        {
            TranscodeImage();
        }

        private void PbSource_ZoomChanged(object sender, EventArgs e)
        {
            // ReSharper disable once LocalizableElement
            tslZoomSource.Text = $"Zoom: {pbSource.Zoom}%";
        }

        private void PbTarget_ZoomChanged(object sender, EventArgs e)
        {
            // ReSharper disable once LocalizableElement
            tslZoomTarget.Text = $"Zoom: {pbTarget.Zoom}%";
        }

        #endregion
    }
}
