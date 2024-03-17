using Microsoft.VisualBasic;
using System;
using System.Resources;
using System.Windows.Forms;

namespace Recursive_Resources_Creator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public string saveFilePath { get; set; } = "";
        public string folderPath { get; set; } = "";

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.ShowDialog();

            saveFilePath = sfd.FileName; // Change this to the desired output path for the .resx file
        }

        static void ProcessDirectory(string directoryPath, ResXResourceWriter writer)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(directoryPath);

            // Recursively process subdirectories
            foreach (var subDir in dirInfo.GetDirectories())
            {
                foreach (var file in subDir.GetFiles())
                {
                    // Generate resource name from file path
                    string resourceName = Path.GetFileNameWithoutExtension(file.FullName);

                    // Read file contents
                    byte[] fileContents = File.ReadAllBytes(file.FullName);

                    // Add file to resources
                    writer.AddResource(resourceName, fileContents);
                    writer.Generate();
                    writer.Close();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog resourcesDirectory = new FolderBrowserDialog();
            resourcesDirectory.ShowDialog();

            folderPath = resourcesDirectory.SelectedPath;

            ResXResourceWriter writer = new ResXResourceWriter(saveFilePath);

            ProcessDirectory(folderPath, writer);
        }
    }
}
