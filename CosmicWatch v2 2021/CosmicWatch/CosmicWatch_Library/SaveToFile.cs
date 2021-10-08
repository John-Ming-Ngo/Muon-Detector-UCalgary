using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.IO;

using Xamarin.Essentials;
using Xamarin.Forms;

//Problem: This doesn't actually save to internal or external storage; but rather to app *local*! That is, storage hidden from users and locked to the app..
//https://stackoverflow.com/questions/59719189/xamarin-forms-how-to-access-data-user-0-com-companyname-notes-files-local-sh
// Stare at https://github.com/CherryBu/FileApp
//https://docs.microsoft.com/en-us/dotnet/api/system.io.streamwriter.autoflush?view=net-5.0
namespace CosmicWatch_Library
{
    public class SaveToFile
    {
        //Current directory for testing purposes: C:\Users\john-\AppData\Local\Packages\b0c704da-8932-48eb-9d48-719b45cebf63_z1sq4jxngb7wr\LocalState

        //Moved to C:\Users\john-\AppData\Local\Packages\b0c704da-8932-48eb-9d48-719b45cebf63_nae1zmw3sy44a\LocalState\CosmicWatchDetectorData

        //Works, but this class is actually sort of dumb and redundent. The better syntax involves calling the base stuff directly and using the using statement, since using ensures proper item disposal...

        private String[] MainDirectoryPathElements;
        private String OutputDirectory;
        private StreamWriter WriteToFile;
        public SaveToFile(String fileName)
        {
            MainDirectoryPathElements = new String[]{ DependencyService.Get<IPlatformDetails>().GetExternalStorageDir(), "CosmicWatchDetectorData"};

            OutputDirectory = Path.Combine(MainDirectoryPathElements);
            Directory.CreateDirectory(OutputDirectory);

            WriteToFile = File.AppendText(Path.Combine(new string[]{OutputDirectory, fileName}));
            
            WriteToFile.AutoFlush = true;
        }

        public void Write(String input)
        {
            WriteToFile.Write(input);
        }
        public void WriteLine(String input)
        {
            WriteToFile.Write(input + "\n");
        }
        public void Close()
        {
            WriteToFile.Close();
        }
        ~SaveToFile()
        {
            Close();
        }

    }
}
