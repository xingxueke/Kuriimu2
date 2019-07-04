namespace Kuriimu2_WinForms.MainForms.Models
{
    // TODO: Replace with ItemWrapper
    class ComboBoxElement
    {
        public object Value { get; }
        public string Name { get; }

        public ComboBoxElement(object value, string name)
        {
            Value = value;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
