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
        private string InputDirectory;
        private string OpenFile;
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
        public ReadFromFile(String ReadFolder)
        {
            String[] MainDirectoryPathElements = { DependencyService.Get<IPlatformDetails>().GetExternalStorageDir(), ReadFolder };
            InputDirectory = Path.Combine(MainDirectoryPathElements);
            Directory.CreateDirectory(InputDirectory);
            
        }

        public bool Open(String Filename)
        {
            OpenFile = Path.Combine(new string[] { InputDirectory, Filename });
            try
            {
                ReadFile = new StreamReader(OpenFile);
            }
            catch (Exception e)
            {
                ReadFile = null;
            }
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
            OpenFile = "";
        }

        public string[] GetFiles()
        {
            string[] Files = Directory.GetFiles(InputDirectory);
            return Files.Select(File => Path.GetFileName(File)).ToArray();
        }
        
        public string GetDirectory()
        {
            return InputDirectory;
        }

        public void DeleteFile()
        {
            ReadFile?.Close();
            if (!File.Exists(OpenFile)) return;
            File.Delete(OpenFile);
            OpenFile = "";
        }

    }
}
