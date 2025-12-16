// Program.cs
using System;
using System.Windows.Forms;

namespace KeyLogger
{
    static class Program
    {
        /// <summary>
        /// Điểm khởi động chính của ứng dụng KeyLogger Investigation Tool.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Kích hoạt visual styles cho ứng dụng WinForms
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Chạy MainForm là giao diện chính
            Application.Run(new MainForm());
        }
    }
}
