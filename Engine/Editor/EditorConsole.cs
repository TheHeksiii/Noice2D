using System;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Editor
{
    public static class EditorConsole
    {
        public static TextBox TextBox { private get; set; }
        public static void Log(string message)
        {

                TextBox.Invoke((Action)(() =>
                {
                    TextBox.AppendText(Environment.NewLine);
                    TextBox.AppendText(message);
                }));



        }
        public static void Clear()
        {
            TextBox.Invoke((Action)(() =>
            {
                TextBox.Clear();
            }));
        }
    }

}

