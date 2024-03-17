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

        public string resxFilePath { get; set; } = "";
        public string folderPath { get; set; } = "";

        public string[] folderPaths { get; set; }


        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog resourcesDirectory = new FolderBrowserDialog();
            if (resourcesDirectory.ShowDialog() == DialogResult.OK)
            {
                folderPath = resourcesDirectory.SelectedPath;
            }
            if (folderPath != null)
            {

                folderPaths.Append(folderPath);
            }
        }

        static void ProcessDirectory(string directoryPath, ResXResourceWriter writer)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(directoryPath);

            foreach (var file in dirInfo.GetFiles())
            {
                // Add file to resources
                AddResource(writer, file.FullName);
            }

            // Recursively process subdirectories
            foreach (var subDir in dirInfo.GetDirectories())
            {
                ProcessDirectory(subDir.FullName, writer);
            }
        }

        static void AddResource(ResXResourceWriter writer, string filePath)
        {
            // Generate resource name from file path
            string resourceName = Path.GetFileNameWithoutExtension(filePath);

            // Read file contents
            byte[] fileContents = File.ReadAllBytes(filePath);

            // Add file to resources
            writer.AddResource(resourceName, fileContents);
        }

        private void button2_Click(object sender, EventArgs e)
        {

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.ShowDialog();

            resxFilePath = sfd.FileName; // Change this to the desired output path for the .resx file

        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (var item in folderPaths)
            {
                try
                {
                    using (ResXResourceWriter writer = new ResXResourceWriter(resxFilePath))
                    {
                        ProcessDirectory(folderPath, writer);
                    }

                    Console.WriteLine("Resource file created successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }
        }
    }
}
