using System.Windows;
using System.Windows.Forms;
using System.IO.Compression;
using System.IO;

namespace ZIPMadness
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private bool isFolderUsed = false;

        private void btnFiles_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Multiselect = true;

            if (ofd.ShowDialog() == true)
            {
                string[] selectedFiles = ofd.FileNames;
                txtItemsToCompress.Text = string.Join(",", selectedFiles);
                isFolderUsed = false;
            }
        }

        private void btnFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            DialogResult result = fbd.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                txtItemsToCompress.Text = fbd.SelectedPath;
                isFolderUsed = true;
            }
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.Filter = "ZIP files (.zip)|*.zip";

            if (!string.IsNullOrEmpty(txtItemsToCompress.Text))
            {
                if (sfd.ShowDialog() == true)
                {
                    if (isFolderUsed)
                    {
                        ZipFile.CreateFromDirectory(txtItemsToCompress.Text, sfd.FileName);
                    }
                    else
                    {
                        string[] files = txtItemsToCompress.Text.Split(',');
                        using (ZipArchive zip = ZipFile.Open(sfd.FileName, ZipArchiveMode.Create))
                        {
                            foreach (string file in files)
                            {
                                zip.CreateEntryFromFile(file, Path.GetFileName(file), CompressionLevel.Optimal);
                            }
                        }
                    }
                    System.Windows.MessageBox.Show("ZIP file created succesfully!!!");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("File(s) or Folder must be selected to create ZIP file!!!");
            }
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();

            if (ofd.ShowDialog() == true)
            {
                using (ZipArchive zip = ZipFile.OpenRead(ofd.FileName))
                {
                    foreach (ZipArchiveEntry entry in zip.Entries)
                    {
                        lbContains.Items.Add(entry.FullName);
                    }
                }
            }
        }

        private void btnExtract_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = "ZIP files (.zip)|*.zip";
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (ofd.ShowDialog() == true)
            {
                txtExtractLocation.Text = ofd.FileName;
                DialogResult result = fbd.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    ZipFile.ExtractToDirectory(ofd.FileName, fbd.SelectedPath);
                    System.Windows.MessageBox.Show("ZIP file extracted succesfully!");
                }
            }
        }
    }
}
