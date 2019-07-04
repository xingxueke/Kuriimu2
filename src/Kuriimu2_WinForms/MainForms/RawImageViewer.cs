using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Kanvas;
using Kanvas.Models;

namespace Kuriimu2_WinForms.MainForms
{
    public partial class RawImageViewer : DynamicImageView
    {
        private bool _fileLoaded;
        private Stream _openedFile;

        public RawImageViewer()
        {
            InitializeComponent();

            LoadMainComboBoxes(cbEncoding, cbSwizzle);

            EncodingPropertyPanel = splExtendedProperties.Panel1;
            SwizzlePropertyPanel = splExtendedProperties.Panel2;
            UpdateExtendedProperties();

            UpdateForm();
        }

        private void LoadImage()
        {
            if (!_fileLoaded || _openedFile == null)
                return;

            if (!int.TryParse(txtOffset.Text, out var offset) &&
                !int.TryParse(txtOffset.Text, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out offset) ||
                !int.TryParse(txtWidth.Text, out var width) ||
                !int.TryParse(txtHeight.Text, out var height))
                return;

            if (offset < 0 || offset >= _openedFile.Length || width <= 0 || height <= 0)
                return;

            ToggleProperties(false);
            ToggleForm(false);

            var imgData = new byte[_openedFile.Length - offset];
            _openedFile.Position = offset;
            _openedFile.Read(imgData, 0, imgData.Length);

            try
            {
                var encoding = CreateColorEncoding(width, height);
                var swizzle = CreateImageSwizzle(width, height);

                var settings = new ImageSettings(encoding, width, height)
                {
                    Swizzle = swizzle
                };
                pbMain.Image = Kolors.Load(imgData, settings);
            }
            catch (TargetInvocationException tie)
            {
                MessageBox.Show(tie.InnerException?.Message, "Encoding or Type parameters invalid.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Exception catched.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            tslPbWidth.Text = pbMain.Image?.Width.ToString() ?? "0";
            tslPbHeight.Text = pbMain.Image?.Height.ToString() ?? "0";

            ToggleProperties(true);
            ToggleForm(true);
        }

        private void OpenFile(string fileName)
        {
            if (!File.Exists(fileName))
                return;

            _openedFile = File.OpenRead(fileName);
            _fileLoaded = true;
        }

        private void UpdateForm()
        {
            cbEncoding.Enabled = cbEncoding.Items.Count > 0;
            cbSwizzle.Enabled = cbSwizzle.Items.Count > 0;

            txtOffset.Enabled = true;
            txtWidth.Enabled = true;
            txtHeight.Enabled = true;

            btnDecode.Enabled = _fileLoaded;
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
            btnDecode.Enabled = toggle;
        }

        #endregion

        #region Events

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog()
            {
                Title = "Open raw image...",
                Filter = "All Files (*.*)|*.*"
            };

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            OpenFile(ofd.FileName);
            LoadImage();
        }

        private void BtnDecode_Click(object sender, EventArgs e)
        {
            LoadImage();
        }

        private void PbMain_ZoomChanged(object sender, EventArgs e)
        {
            tslZoom.Text = $"Zoom: {pbMain.Zoom}%";
        }

        #endregion
    }
}
