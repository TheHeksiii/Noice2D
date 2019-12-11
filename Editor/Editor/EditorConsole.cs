using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Editor
{
    public static class EditorConsole
    {
        public static List<string> preparedMessages = new List<string>();
        public static TextBox TextBox { private get; set; }
        public static void Log(object message)
        {
            if (Editor.GetInstance().IsHandleCreated == false)
            {
                preparedMessages.Add(message.ToString());
            }
            else
            {
                TextBox.Invoke((Action)(() =>
                {
                    for (int i = 0; i < preparedMessages.Count; i++)
                    {
                        TextBox.AppendText(Environment.NewLine);
                        TextBox.AppendText(preparedMessages[i]);
                        preparedMessages.RemoveAt(0);
                    }
                    preparedMessages.Clear();
                    TextBox.AppendText(Environment.NewLine);
                    TextBox.AppendText(message.ToString());
                }));
            }


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

