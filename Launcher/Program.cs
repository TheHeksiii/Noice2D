using System;
using System.Windows.Forms;
namespace Launcher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            bool runEditor = true;
            if (runEditor)
			{ 
				Application.Run(new Editor.EditorWindow());
            }
            else
            {
                var scene = new Engine.Scene();
                scene.Run();

            }
        }
    }
}
