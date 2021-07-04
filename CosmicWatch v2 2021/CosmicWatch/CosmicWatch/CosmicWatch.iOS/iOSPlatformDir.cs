using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

using CosmicWatch.iOS;
using Xamarin.Forms;
using CosmicWatch_Library;
using Xamarin.Essentials;

[assembly: Dependency(typeof(iOSPlatformDir))]
namespace CosmicWatch.iOS
{
    class iOSPlatformDir : IPlatformDetails
    {
        public String GetExternalStorageDir()
        {
            return FileSystem.AppDataDirectory;
        }
        public String GetInternalStorageDir()
        {
            return FileSystem.AppDataDirectory;
        }
    }
}