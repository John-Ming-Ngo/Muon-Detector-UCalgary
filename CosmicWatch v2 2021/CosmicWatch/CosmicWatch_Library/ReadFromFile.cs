using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CosmicWatch_Library
{
    public class ReadFromFile
    {
        private String[] MainDirectoryPathElements = { DependencyService.Get<IPlatformDetails>().GetExternalStorageDir(), "CosmicWatchDetectorData" };
        private string InputDirectory;
        private StreamReader ReadFile;

        public bool EndOfFile
        {
            get
            {
                if (ReadFile != null) return ReadFile.EndOfStream;
                else return true;
            }
        }

        //Todo: String directory input.
        public ReadFromFile()
        {
            InputDirectory = Path.Combine(MainDirectoryPathElements);
            Directory.CreateDirectory(InputDirectory);
            
        }

        public bool Open(String Filename)
        {
            ReadFile = new StreamReader(Path.Combine(new string[] { InputDirectory, Filename }));
            return ReadFile != null;
        }
        public string ReadLine()
        {
            string line = "";
            line = ReadFile?.ReadLine();
            return line;
        }
        public void Close()
        {
            ReadFile?.Close();
        }

        public string[] GetFiles()
        {
            string[] Files = Directory.GetFiles(InputDirectory);
            return Files.Select(File => Path.GetFileName(File)).ToArray();
        }
    }
}
