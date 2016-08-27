using ReflectionApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StringsClassLibrary
{
    public class Strings : IReflection
    {
        public string Caption { get; } = "Strings";

        public UserControl GetControl()
        {
            return new StringsUserControl();
        }
    }
}
