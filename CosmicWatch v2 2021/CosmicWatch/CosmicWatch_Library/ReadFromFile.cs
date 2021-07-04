using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CosmicWatch_Library
{
    public class ReadFromFile
    {
        private String[] MainDirectoryPathElements = { DependencyService.Get<IPlatformDetails>().GetExternalStorageDir(), "CosmicWatchDetectorData" };
        private string OutputDirectory;
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
            OutputDirectory = Path.Combine(MainDirectoryPathElements);
            Directory.CreateDirectory(OutputDirectory);
            
        }

        public bool Open(String Filename)
        {
            ReadFile = new StreamReader(Path.Combine(new string[] { OutputDirectory, Filename }));
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
            return Directory.GetFiles(Path.Combine(MainDirectoryPathElements));
        }
    }
}
