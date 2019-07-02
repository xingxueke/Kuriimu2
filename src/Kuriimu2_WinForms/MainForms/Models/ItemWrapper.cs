using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kuriimu2_WinForms.MainForms.Models
{
    internal class ItemWrapper<T>
    {
        public T Value { get; }

        public string Name { get; }

        public ItemWrapper(T value, string name)
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
