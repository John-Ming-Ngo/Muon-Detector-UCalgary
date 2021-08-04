﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CosmicWatch.UWP;
using CosmicWatch_Library;
using Xamarin.Forms;
using Xamarin.Essentials;

[assembly: Dependency(typeof(UWPPlatformDir))]
namespace CosmicWatch.UWP
{
    /*
     Purpose: 
        
    Organization of this page:
        1. Data and References
        2. Functionality.

    Todo: Document this class and use it somewhere.
     */
    class UWPPlatformDir : IPlatformDetails
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
