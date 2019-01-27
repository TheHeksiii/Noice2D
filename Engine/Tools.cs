using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public static class Tools
    {
        public enum ToolTypes { Select, Text, Polygon, Line, Box, Circle };
        public static ToolTypes CurrentTool { get; private set; }

        public static void NextTool()
        {
            CurrentTool++;
            if ((int)CurrentTool > 5)
            {
                CurrentTool = (ToolTypes)0;
            }
        }
        public static void PreviousTool()
        {
            CurrentTool--;
            if (CurrentTool < 0)
            {
                CurrentTool = (ToolTypes)5;
            }
        }
        public static void SetSelectTool()
        {
            CurrentTool = 0;
        }
    }
}
