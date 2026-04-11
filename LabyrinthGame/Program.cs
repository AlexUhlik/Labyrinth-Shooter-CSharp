using System;
using System.Windows.Forms; // 1. Это пространство имен ОБЯЗАТЕЛЬНО

namespace LabyrinthGame
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
        
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            System.Windows.Forms.Application.Run(new TestWindow());
        }
    }
}