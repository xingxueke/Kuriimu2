using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Kanvas.Interface;
using Kuriimu2_WinForms.MainForms.Models;

namespace Kuriimu2_WinForms.MainForms
{
    /// <summary>
    /// Holds all shared methods used in the specialized image views.
    /// </summary>
    public class DynamicImageView : Form
    {
        protected Panel EncodingPropertyPanel { get; set; }
        protected Panel SwizzlePropertyPanel { get; set; }

        protected readonly Assembly Kanvas;

        protected Type SelectedColorEncoding { get; private set; }

        protected Type SelectedSwizzle { get; private set; }

        public DynamicImageView()
        {
            Kanvas = Assembly.GetAssembly(typeof(IColorEncoding));
        }

        #region Load

        protected void LoadMainComboBoxes(ComboBox cbEncoding, ComboBox cbSwizzle)
        {
            LoadEncodings(cbEncoding);
            LoadSwizzles(cbSwizzle);

            cbEncoding.SelectedIndexChanged += EncodingSelectedIndexChanged;
            cbSwizzle.SelectedIndexChanged += SwizzleSelectedIndexChanged;

            cbEncoding.SelectedIndex = 0;
            cbSwizzle.SelectedIndex = 0;
        }

        private void LoadEncodings(ComboBox cbEncoding)
        {
            cbEncoding.Items.Clear();

            // Populate encoding dropdown
            foreach (var encoding in GetKanvasTypes<IColorEncoding>())
                cbEncoding.Items.Add(new ItemWrapper<Type>(encoding, encoding.Name));
        }

        private void LoadSwizzles(ComboBox cbSwizzle)
        {
            cbSwizzle.Items.Clear();

            // Set 'None' Swizzle
            cbSwizzle.Items.Add(new ItemWrapper<Type>(null, "None"));

            // Populate swizzle dropdown
            foreach (var swizzle in GetKanvasTypes<IImageSwizzle>())
                cbSwizzle.Items.Add(new ItemWrapper<Type>(swizzle, swizzle.Name.Replace("Swizzle", "")));

            // Set 'Custom' swizzle
            cbSwizzle.Items.Add(new ItemWrapper<Type>(typeof(CustomSwizzle), "Custom"));
        }

        #endregion

        #region Events

        private void EncodingSelectedIndexChanged(object sender, EventArgs e)
        {
            var cmb = sender as ComboBox;
            SelectedColorEncoding = (cmb?.Items[cmb.SelectedIndex] as ItemWrapper<Type>)?.Value;
            UpdateEncodingProperties();
        }

        private void SwizzleSelectedIndexChanged(object sender, EventArgs e)
        {
            var cmb = sender as ComboBox;
            SelectedSwizzle = (cmb?.Items[cmb.SelectedIndex] as ItemWrapper<Type>)?.Value;
            UpdateSwizzleProperties();
        }

        #endregion

        #region Updates

        protected void UpdateExtendedProperties()
        {
            UpdateEncodingProperties();
            UpdateSwizzleProperties();
        }

        private void UpdateEncodingProperties()
        {
            if (EncodingPropertyPanel == null)
                return;

            EncodingPropertyPanel.Controls.Clear();

            if (SelectedColorEncoding != null)
                UpdateExtendedPropertiesWith(
                    EncodingPropertyPanel,
                    80,
                    SelectedColorEncoding.GetConstructors().LastOrDefault()?.GetParameters());
        }

        private void UpdateSwizzleProperties()
        {
            if (SwizzlePropertyPanel == null)
                return;

            SwizzlePropertyPanel.Controls.Clear();

            if (SelectedSwizzle != null)
                UpdateExtendedPropertiesWith(
                    SwizzlePropertyPanel,
                    80,
                    SelectedSwizzle.GetConstructors().LastOrDefault()?.GetParameters());
        }

        private void UpdateExtendedPropertiesWith(Panel panel, int width, ParameterInfo[] parameters)
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

        private void AddBooleanParameter(Panel panel, int width, ParameterInfo parameter, int x, int y)
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
                Location = new Point(x, y + label.Height),
                Size = new Size(15, 20)
            };
            checkBox.Checked = false;

            panel.Controls.Add(label);
            panel.Controls.Add(checkBox);
        }

        private void AddEnumParameter(Panel panel, int width, ParameterInfo parameter, int x, int y)
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

        private void AddParameter(Panel panel, int width, ParameterInfo parameter, int x, int y)
        {
            if (parameter.ParameterType == typeof(bool) || parameter.ParameterType.IsEnum)
                return;

            var name = MakeFirstLetterUppercCase(parameter.Name);
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

        #endregion

        #region Activator

        protected IColorEncoding CreateColorEncoding(int width, int height)
        {
            var values = GetParameterValues(EncodingPropertyPanel, SelectedColorEncoding, width, height);

            return (IColorEncoding)Activator.CreateInstance(SelectedColorEncoding, values.ToArray());
        }

        protected IImageSwizzle CreateImageSwizzle(int width, int height)
        {
            if (SelectedSwizzle == null)
                return null;

            var values = GetParameterValues(SwizzlePropertyPanel, SelectedSwizzle, width, height);

            return (IImageSwizzle)Activator.CreateInstance(SelectedSwizzle, values.ToArray());
        }

        private IEnumerable<object> GetParameterValues(Panel panel, Type type, int width, int height)
        {
            var parameters = type.GetConstructors().LastOrDefault()?.GetParameters();
            if (parameters == null)
                yield break;

            foreach (var parameter in parameters)
            {
                var name = MakeFirstLetterUppercCase(parameter.Name);
                if (parameter.ParameterType == typeof(bool))
                    yield return panel.Controls.Find($"chk{name}", false).Cast<CheckBox>()
                        .FirstOrDefault()?.Checked;
                else if (parameter.ParameterType.IsEnum)
                    yield return (panel.Controls.Find($"cmb{name}", false).Cast<ComboBox>()
                        .FirstOrDefault()?.SelectedItem as ItemWrapper<object>)?.Value;
                else
                {
                    if (name == "Width")
                        yield return width;
                    else if (name == "Height")
                        yield return height;
                    else
                    {
                        var textBox = panel.Controls.Find($"txt{name}", false).Cast<TextBox>()
                            .FirstOrDefault();

                        if (name == "BitField")
                            yield return ParseBitField(textBox.Text);
                        else
                            yield return Convert.ChangeType(textBox.Text, parameter.ParameterType);
                    }
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

        #endregion

        #region Utilities

        private IEnumerable<Type> GetKanvasTypes<TKanvas>()
        {
            return Kanvas.GetTypes().Where(x => typeof(TKanvas).IsAssignableFrom(x) && !x.IsInterface);
        }

        private string MakeFirstLetterUppercCase(string value)
        {
            return $"{char.ToUpper(value[0])}{value.Substring(1)}";
        }

        #endregion
    }
}
