using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReflectionApplication
{
    public interface IReflection
    {
        String Caption { get; }

        UserControl GetControl();
    }
}
