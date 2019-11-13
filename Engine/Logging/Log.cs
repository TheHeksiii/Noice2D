using System.Windows.Forms;

public static class Log
    {
        public static void Error(string message)
        {
            MessageBox.Show(message);
        }
    }
