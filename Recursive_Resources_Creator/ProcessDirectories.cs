using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Recursive_Resources_Creator
{
    internal class ProcessDirectories
    {

        public ProcessDirectories()
        {

            FolderBrowserDialog resourcesDirectory = new FolderBrowserDialog();
            resourcesDirectory.ShowDialog();

            DirectoryInfo dirInfo = new DirectoryInfo(resourcesDirectory.SelectedPath);

            Dictionary<string, object> filesInDirectory = new Dictionary<string, object>();

            // Recursively process subdirectories
            foreach (var subDir in dirInfo.GetDirectories())
            {
                filesInDirectory.Add(subDir.FullName, subDir.EnumerateFiles());
            }
        }

        public static void AddResource(Dictionary<string, object> res, string inputPath, string outputPath)
        {
            List<string> SkipList = new List<string>();

            var dic = new Dictionary<string, ResXDataNode>();
            using (var reader = new ResXResourceReader(inputPath) { UseResXDataNodes = true })
            {

                dic = reader.Cast<DictionaryEntry>().ToDictionary(e => e.Key.ToString(), e => e.Value as ResXDataNode);
                foreach (var kv in res)
                {
                    if (!dic.ContainsKey(kv.Key) && !SkipList.Contains(kv.Key))
                    {
                        dic.Add(kv.Key, new ResXDataNode(kv.Key, kv.Value) { Comment = kv.Value.ToString() });
                    }
                }
            }

            using (var writer = new ResXResourceWriter(outputPath))
            {
                foreach (var kv in dic)
                {
                    writer.AddResource(kv.Key, kv.Value);
                }

                writer.Generate();
            }
        }
    }
}
