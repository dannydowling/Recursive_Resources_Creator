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

        public string folderPath { get; set; } = "";
   
        static void ProcessDirectory(string directoryPath)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(directoryPath);
            
            var saveFilePath = $@"c:\resources\{dirInfo.Name}.resx";

            ResXResourceWriter writer = new ResXResourceWriter(saveFilePath);

            // Recursively process subdirectories

            foreach (var file in dirInfo.GetFiles())
                {
                    // Generate resource name from file path
                    string resourceName = Path.GetFileNameWithoutExtension(file.FullName);

                    // Read file contents
                    byte[] fileContents = File.ReadAllBytes(file.FullName);

                    // Add file to resources
                    writer.AddResource(resourceName, fileContents);                    
                }
                writer.Generate();
                writer.Close();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog resourcesDirectory = new FolderBrowserDialog();
            resourcesDirectory.ShowDialog();

            folderPath = resourcesDirectory.SelectedPath;

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(folderPath);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);            
        }
    }
}
