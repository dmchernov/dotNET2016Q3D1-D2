using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace ReflectionApplication
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            var files = Directory.GetFiles("Modules", "*.dll", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                Assembly myAssembly = Assembly.LoadFrom(file);
                Type type =
                    myAssembly.GetTypes().Where(t => t.IsClass && t.GetInterface(typeof(IReflection).ToString()) == typeof(IReflection)).First();

                if (type == null) continue;

                IReflection r = (IReflection) Activator.CreateInstance(type);

                UserControl uc = r.GetControl();
                uc.Dock = DockStyle.Fill;

                mainMenuStrip.Items.Add(r.Caption, null, (o, args) =>
                {
                    foreach (Control control in Controls)
                    {
                        if (control != uc && control != mainMenuStrip)
                            Controls.Remove(control);
                    }
                    if (!Controls.Contains(uc))
                        Controls.Add(uc);
                });
            }
        }
    }
}
