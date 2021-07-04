using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;

using CosmicWatch;
using CosmicWatch.Droid;
using CosmicWatch_Library;

[assembly: Dependency(typeof(AndroidPlatformDetails))]

namespace CosmicWatch.Droid
{
    public class AndroidPlatformDetails : IPlatformDetails
    {
        public String GetExternalStorageDir()
        {
            Context context = Android.App.Application.Context;
            Java.IO.File filePath = context.GetExternalFilesDir(""); 
            return filePath.Path;
        }
        public String GetInternalStorageDir()
        {
            Context context = Android.App.Application.Context;
            Java.IO.File filePath = context.GetExternalFilesDir(""); //Same for now.
            return filePath.Path;
        }
    }
}