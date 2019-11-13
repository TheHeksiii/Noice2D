using System.Collections.Generic;
using System.IO;

namespace Editor
{
    public static class EditorResources
    {
        public static Dictionary<string, string> Icons;

        static EditorResources()
        {
            Icons = new Dictionary<string, string>()
            {
                { "Folder", GetResourcePath("Icons", "FolderIcon.png") }
            };
        }
        private static string GetResourcePath(string category, string name)
        {
            return (Path.Combine(Directory.GetCurrentDirectory(), "EditorResources", category, name));
        }
    }
}
