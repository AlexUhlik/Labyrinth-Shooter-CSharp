using System;
using System.Windows.Forms; // 1. Это пространство имен ОБЯЗАТЕЛЬНО

namespace LabyrinthGame
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // 2. Вызываем МЕТОДЫ класса Application через скобки ()
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            // 3. Запускаем само приложение, передавая объект вашей формы
            System.Windows.Forms.Application.Run(new Form1());
        }
    }
}