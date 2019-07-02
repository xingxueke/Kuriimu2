using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Kanvas;
using Kanvas.Interface;
using Kanvas.Models;
using Kontract;
using Kontract.Attributes.Intermediate;
using Kontract.Interfaces.Intermediate;
using Kontract.Models;
using Kuriimu2_WinForms.MainForms.Models;

namespace Kuriimu2_WinForms.MainForms
{
    public partial class RawImageViewer : Form
    {
        private readonly PluginLoader _loader;

        private bool _fileLoaded;
        private Stream _openedFile;

        private int _selectedEncodingIndex;
        private int _selectedSwizzleIndex;

        private readonly SplitterPanel _pnlEncodingProperties;
        private readonly SplitterPanel _pnlSwizzleProperties;

        private Type SelectedColorEncoding
        {
            get
            {
                if (_selectedEncodingIndex < cbEncoding.Items.Count)
                    return (cbEncoding.Items[_selectedEncodingIndex] as ItemWrapper<Type>)?.Value;

                return null;
            }
        }

        private Type SelectedSwizzle
        {
            get
            {
                if (_selectedSwizzleIndex < cbSwizzle.Items.Count)
                    return (cbSwizzle.Items[_selectedSwizzleIndex] as ItemWrapper<Type>)?.Value;

                return null;
            }
        }

        public RawImageViewer(PluginLoader loader)
        {
            InitializeComponent();

            _loader = loader;
            _pnlEncodingProperties = splExtendedProperties.Panel1;
            _pnlSwizzleProperties = splExtendedProperties.Panel2;

            cbEncoding.SelectedIndexChanged -= CbEncoding_SelectedIndexChanged;
            cbSwizzle.SelectedIndexChanged -= CbSwizzle_SelectedIndexChanged;
            LoadComboBoxes();
            cbEncoding.SelectedIndexChanged += CbEncoding_SelectedIndexChanged;
            cbSwizzle.SelectedIndexChanged += CbSwizzle_SelectedIndexChanged;

            UpdateForm();
            UpdateExtendedProperties();
        }

        private void LoadComboBoxes()
        {
            LoadEncodings();
            LoadSwizzles();
        }

        private void LoadEncodings()
        {
            // Populate encoding dropdown
            var encodings = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IColorEncoding).IsAssignableFrom(p) && !p.IsInterface);
            foreach (var encoding in encodings)
                cbEncoding.Items.Add(new ItemWrapper<Type>(encoding, encoding.Name));

            if (_selectedEncodingIndex < cbEncoding.Items.Count)
                cbEncoding.SelectedIndex = _selectedEncodingIndex;
        }

        private void LoadSwizzles()
        {
            // Set 'None' Swizzle
            cbSwizzle.Items.Add(new ItemWrapper<Type>(null, "None"));

            // TODO: Double Custom swizzle in combobox
            // Populate swizzle dropdown
            var swizzles = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IImageSwizzle).IsAssignableFrom(p) && !p.IsInterface);
            foreach (var swizzle in swizzles)
                cbSwizzle.Items.Add(new ItemWrapper<Type>(swizzle, swizzle.Name.Replace("Swizzle", "")));

            if (_selectedSwizzleIndex < cbSwizzle.Items.Count)
                cbSwizzle.SelectedIndex = _selectedSwizzleIndex;
        }

        private void CbEncoding_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedEncodingIndex = cbEncoding.SelectedIndex;
            UpdateEncodingProperties();
        }

        private void CbSwizzle_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedSwizzleIndex = cbSwizzle.SelectedIndex;
            UpdateSwizzleProperty();
        }

        #region Update

        private void UpdateForm()
        {
            cbEncoding.Enabled = cbEncoding.Items.Count > 0;
            cbSwizzle.Enabled = cbSwizzle.Items.Count > 0;
            txtOffset.Enabled = true;
            btnDecode.Enabled = _fileLoaded;
        }

        private void UpdateExtendedProperties()
        {
            // TODO: Find all parameters of ctor, even optionals
            UpdateEncodingProperties();
            UpdateSwizzleProperty();
        }

        private void UpdateEncodingProperties()
        {
            _pnlEncodingProperties.Controls.Clear();

            if (SelectedColorEncoding != null)
                UpdateExtendedPropertiesWith(
                    _pnlEncodingProperties,
                    80,
                    SelectedColorEncoding.GetConstructors().FirstOrDefault()?.GetParameters());
        }

        private void UpdateSwizzleProperty()
        {
            _pnlSwizzleProperties.Controls.Clear();

            //if (SelectedSwizzle != null && SelectedSwizzle.Name == "Custom")
            //{
            //    var label = new Label
            //    {
            //        // ReSharper disable once LocalizableElement
            //        Text = "Bit Field:",
            //        Location = new Point(3, 0),
            //        Size = new Size(200, 15)
            //    };
            //    var textBox = new TextBox
            //    {
            //        Name = "txtBitField",
            //        Location = new Point(3, 0 + label.Height),
            //        Size = new Size(200, 20)
            //    };

            //    _pnlSwizzleProperties.Controls.Add(label);
            //    _pnlSwizzleProperties.Controls.Add(textBox);
            //    textBox.TextChanged += CustomSwizzleBitField_TextChanged;
            //}

            if (SelectedSwizzle != null)
                UpdateExtendedPropertiesWith(
                    _pnlSwizzleProperties,
                    80,
                    SelectedSwizzle.GetConstructors().FirstOrDefault()?.GetParameters());
        }

        //private void CustomSwizzleBitField_TextChanged(object sender, EventArgs e)
        //{
        //    var tb = (TextBox)sender;
        //    var prop = (PropertyInfo)tb.Tag;

        //    var splitted = Regex.Split(tb.Text, "\\)[ ]*,[ ]*\\(").Select(x => Regex.Match(x, "\\d+[ ]*,[ ]*\\d+").Value).ToArray();
        //    if (splitted.Any(x => string.IsNullOrEmpty(x)))
        //        return;

        //    prop.SetValue(SelectedSwizzle, splitted.Select(x =>
        //    {
        //        var internalSplit = Regex.Split(x, "[ ]*,[ ]*").ToArray();
        //        return (int.Parse(internalSplit[0]), int.Parse(internalSplit[1]));
        //    }).ToList());
        //}

        private void UpdateExtendedPropertiesWith(SplitterPanel panel, int width, ParameterInfo[] parameters)
        {
            int x = 3;
            foreach (var parameter in parameters)
            {
                if (parameter.Name == "width" || parameter.Name == "height")
                    continue;

                if (parameter.ParameterType == typeof(bool))
                    AddBooleanParameter(panel, width, parameter, x, 0);
                else if (parameter.ParameterType.IsEnum)
                    AddEnumParameter(panel, width, parameter, x, 0);
                else
                    AddParameter(panel, width, parameter, x, 0);

                x += width + 3;
            }
        }

        private void AddBooleanParameter(SplitterPanel panel, int width, ParameterInfo parameter, int x, int y)
        {
            if (parameter.ParameterType != typeof(bool))
                return;

            var name = $"{char.ToUpper(parameter.Name[0])}{parameter.Name.Substring(1)}";
            var label = new Label
            {
                Text = $"{name}:",
                Location = new Point(x, y),
                Size = new Size(width, 15)
            };
            var checkBox = new CheckBox
            {
                Name = $"chk{name}",
                Location = new Point(x, y + label.Height)
            };
            checkBox.Checked = false;

            panel.Controls.Add(label);
            panel.Controls.Add(checkBox);
        }

        private void AddEnumParameter(SplitterPanel panel, int width, ParameterInfo parameter, int x, int y)
        {
            if (!parameter.ParameterType.IsEnum)
                return;

            var formatWrappers = Enum.GetNames(parameter.ParameterType).
                Zip(Enum.GetValues(parameter.ParameterType).Cast<object>(), Tuple.Create).
                Select(enumValue => (object)new ItemWrapper<object>(enumValue.Item2, enumValue.Item1)).
                ToArray();

            var name = $"{char.ToUpper(parameter.Name[0])}{parameter.Name.Substring(1)}";
            var label = new Label
            {
                Text = $"{name}:",
                Location = new Point(x, y),
                Size = new Size(width, 15)
            };
            var comboBox = new ComboBox
            {
                Name = $"cmb{name}",
                Location = new Point(x, y + label.Height),
                Size = new Size(width, 20)
            };
            comboBox.Items.AddRange(formatWrappers);
            comboBox.SelectedIndex = 0;

            panel.Controls.Add(label);
            panel.Controls.Add(comboBox);
        }

        private void AddParameter(SplitterPanel panel, int width, ParameterInfo parameter, int x, int y)
        {
            if (parameter.ParameterType == typeof(bool) || parameter.ParameterType.IsEnum)
                return;

            var name = $"{char.ToUpper(parameter.Name[0])}{parameter.Name.Substring(1)}";
            var label = new Label
            {
                Text = $"{name}:",
                Location = new Point(x, y),
                Size = new Size(width, 15)
            };
            var textBox = new TextBox
            {
                Name = $"txt{name}",
                Location = new Point(x, y + label.Height),
                Size = new Size(width, 20)
            };

            textBox.Text = parameter.ParameterType.IsPrimitive ? "8" : string.Empty;

            panel.Controls.Add(label);
            panel.Controls.Add(textBox);
        }

        //private void EncodingPropertyTextBox_TextChanged(object sender, EventArgs e)
        //{
        //    var tb = (TextBox)sender;
        //    var propAttr = (PropertyAttribute)tb.Tag;

        //    object value;
        //    switch (Type.GetTypeCode(propAttr.PropertyType))
        //    {
        //        case TypeCode.Byte:
        //            byte val1;
        //            if (!byte.TryParse(tb.Text, out val1))
        //                return;
        //            value = val1;
        //            break;
        //        case TypeCode.SByte:
        //            sbyte val2;
        //            if (!sbyte.TryParse(tb.Text, out val2))
        //                return;
        //            value = val2;
        //            break;
        //        case TypeCode.Int16:
        //            short val3;
        //            if (!short.TryParse(tb.Text, out val3))
        //                return;
        //            value = val3;
        //            break;
        //        case TypeCode.UInt16:
        //            ushort val4;
        //            if (!ushort.TryParse(tb.Text, out val4))
        //                return;
        //            value = val4;
        //            break;
        //        case TypeCode.Int32:
        //            int val5;
        //            if (!int.TryParse(tb.Text, out val5))
        //                return;
        //            value = val5;
        //            break;
        //        case TypeCode.UInt32:
        //            uint val6;
        //            if (!uint.TryParse(tb.Text, out val6))
        //                return;
        //            value = val6;
        //            break;
        //        case TypeCode.Int64:
        //            long val7;
        //            if (!long.TryParse(tb.Text, out val7))
        //                return;
        //            value = val7;
        //            break;
        //        case TypeCode.UInt64:
        //            ulong val8;
        //            if (!ulong.TryParse(tb.Text, out val8))
        //                return;
        //            value = val8;
        //            break;
        //        default:
        //            return;
        //    }

        //    var adapterProperty = SelectedColorEncoding.GetType()
        //        .GetProperty(propAttr.PropertyName, propAttr.PropertyType);
        //    adapterProperty?.SetValue(SelectedColorEncoding, value);
        //}

        //private void SwizzlePropertyTextBox_TextChanged(object sender, EventArgs e)
        //{
        //    var tb = (TextBox)sender;
        //    var propAttr = (PropertyAttribute)tb.Tag;

        //    object value;
        //    switch (Type.GetTypeCode(propAttr.PropertyType))
        //    {
        //        case TypeCode.Byte:
        //            byte val1;
        //            if (!byte.TryParse(tb.Text, out val1))
        //                return;
        //            value = val1;
        //            break;
        //        case TypeCode.SByte:
        //            sbyte val2;
        //            if (!sbyte.TryParse(tb.Text, out val2))
        //                return;
        //            value = val2;
        //            break;
        //        case TypeCode.Int16:
        //            short val3;
        //            if (!short.TryParse(tb.Text, out val3))
        //                return;
        //            value = val3;
        //            break;
        //        case TypeCode.UInt16:
        //            ushort val4;
        //            if (!ushort.TryParse(tb.Text, out val4))
        //                return;
        //            value = val4;
        //            break;
        //        case TypeCode.Int32:
        //            int val5;
        //            if (!int.TryParse(tb.Text, out val5))
        //                return;
        //            value = val5;
        //            break;
        //        case TypeCode.UInt32:
        //            uint val6;
        //            if (!uint.TryParse(tb.Text, out val6))
        //                return;
        //            value = val6;
        //            break;
        //        case TypeCode.Int64:
        //            long val7;
        //            if (!long.TryParse(tb.Text, out val7))
        //                return;
        //            value = val7;
        //            break;
        //        case TypeCode.UInt64:
        //            ulong val8;
        //            if (!ulong.TryParse(tb.Text, out val8))
        //                return;
        //            value = val8;
        //            break;
        //        default:
        //            return;
        //    }

        //    var adapterProperty = SelectedSwizzle.GetType()
        //        .GetProperty(propAttr.PropertyName, propAttr.PropertyType);
        //    adapterProperty?.SetValue(SelectedSwizzle, value);
        //}

        //private void EncodingPropertyCheckBox_CheckedChanged(object sender, EventArgs e)
        //{
        //    var cb = (CheckBox)sender;
        //    var propAttr = (PropertyAttribute)cb.Tag;

        //    var adapterProperty = SelectedColorEncoding.GetType()
        //        .GetProperty(propAttr.PropertyName, propAttr.PropertyType);
        //    adapterProperty?.SetValue(SelectedColorEncoding, cb.Checked);
        //}

        //private void SwizzlePropertyCheckBox_CheckedChanged(object sender, EventArgs e)
        //{
        //    var cb = (CheckBox)sender;
        //    var propAttr = (PropertyAttribute)cb.Tag;

        //    var adapterProperty = SelectedSwizzle.GetType()
        //        .GetProperty(propAttr.PropertyName, propAttr.PropertyType);
        //    adapterProperty?.SetValue(SelectedSwizzle, cb.Checked);
        //}

        //private void EncodingPropertyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    var cb = (ComboBox)sender;
        //    var index = cb.SelectedIndex;
        //    var format = (FormatWrapper)cb.Items[index];

        //    var propAttr = (PropertyAttribute)cb.Tag;

        //    var adapterProperty = SelectedColorEncoding.GetType()
        //        .GetProperty(propAttr.PropertyName, propAttr.PropertyType);
        //    adapterProperty?.SetValue(SelectedColorEncoding, format.Value);
        //}

        //private void SwizzlePropertyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    var cb = (ComboBox)sender;
        //    var index = cb.SelectedIndex;
        //    var format = (FormatWrapper)cb.Items[index];

        //    var propAttr = (PropertyAttribute)cb.Tag;

        //    var adapterProperty = SelectedSwizzle.GetType()
        //        .GetProperty(propAttr.PropertyName, propAttr.PropertyType);
        //    adapterProperty?.SetValue(SelectedSwizzle, format.Value);
        //}

        #endregion

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

            var progress = new Progress<ProgressReport>();
            try
            {
                // TODO: Test actual decoding
                var encoding = CreateColorEncoding(SelectedColorEncoding);
                var settings = new ImageSettings(encoding, width, height)
                {
                    // TODO: Create swizzle beforehand
                    Swizzle = null,
                };
                pbMain.Image = Kolors.Load(imgData, settings);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Exception catched.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            tslPbWidth.Text = pbMain.Image.Width.ToString();
            tslPbHeight.Text = pbMain.Image.Height.ToString();

            ToggleProperties(true);
            ToggleForm(true);
        }

        private void ToggleProperties(bool toggle)
        {
            var textBoxes = splExtendedProperties.Panel1.Controls.OfType<TextBox>();
            var comboBoxes = splExtendedProperties.Panel1.Controls.OfType<ComboBox>();
            var checkBoxes = splExtendedProperties.Panel1.Controls.OfType<CheckBox>();

            foreach (var textBox in textBoxes)
                textBox.Enabled = toggle;
            foreach (var comboBox in comboBoxes)
                comboBox.Enabled = toggle;
            foreach (var checkBox in checkBoxes)
                checkBox.Enabled = toggle;

            textBoxes = splExtendedProperties.Panel2.Controls.OfType<TextBox>();
            comboBoxes = splExtendedProperties.Panel2.Controls.OfType<ComboBox>();
            checkBoxes = splExtendedProperties.Panel2.Controls.OfType<CheckBox>();

            foreach (var textBox in textBoxes)
                textBox.Enabled = toggle;
            foreach (var comboBox in comboBoxes)
                comboBox.Enabled = toggle;
            foreach (var checkBox in checkBoxes)
                checkBox.Enabled = toggle;
        }

        private void ToggleForm(bool toggle)
        {
            cbEncoding.Enabled = toggle;
            cbSwizzle.Enabled = toggle;
            openToolStripMenuItem.Enabled = toggle;
            btnDecode.Enabled = toggle;
        }

        private IColorEncoding CreateColorEncoding(Type encodingType)
        {
            var values = GetEncodingParameterValues(encodingType);

            return (IColorEncoding)Activator.CreateInstance(encodingType, values);
        }

        private IEnumerable<object> GetEncodingParameterValues(Type encodingType)
        {
            var parameters = encodingType.GetConstructors().FirstOrDefault()?.GetParameters();
            if (parameters == null)
                yield break;

            foreach (var parameter in parameters)
            {
                var name = $"{char.ToUpper(parameter.Name[0])}{parameter.Name.Substring(1)}";
                if (parameter.ParameterType == typeof(bool))
                    yield return _pnlEncodingProperties.Controls.Find($"chk{name}", false).Cast<CheckBox>()
                        .FirstOrDefault()?.Checked;
                else if (parameter.ParameterType.IsEnum)
                    yield return (_pnlEncodingProperties.Controls.Find($"cmb{name}", false).Cast<ComboBox>()
                        .FirstOrDefault()?.SelectedItem as ItemWrapper<object>)?.Value;
                else
                {
                    var textBox = _pnlEncodingProperties.Controls.Find($"txt{name}", false).Cast<TextBox>()
                        .FirstOrDefault();

                    // TODO: Move bitField special handling to swizzle creation
                    if (parameter.ParameterType.Name == "bitField")
                        yield return ParseBitField(textBox.Text);
                    yield return Convert.ChangeType(textBox.Text, parameter.ParameterType);
                }
            }
        }

        private (int, int)[] ParseBitField(string text)
        {
            var splitted = Regex.Split(text, "\\)[ ]*,[ ]*\\(").Select(x => Regex.Match(x, "\\d+[ ]*,[ ]*\\d+").Value).ToArray();
            if (splitted.Any(x => string.IsNullOrEmpty(x)))
                throw new InvalidOperationException("BitField is not given.");

            return splitted.Select(x =>
            {
                var internalSplit = Regex.Split(x, "[ ]*,[ ]*").ToArray();
                return (int.Parse(internalSplit[0]), int.Parse(internalSplit[1]));
            }).ToArray();
        }

        private void OpenFile(string fileName)
        {
            if (!File.Exists(fileName))
                return;

            _openedFile = File.OpenRead(fileName);
            _fileLoaded = true;
        }

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
            // ReSharper disable once LocalizableElement
            tslZoom.Text = $"Zoom: {pbMain.Zoom}%";
        }

        #endregion
    }
}
