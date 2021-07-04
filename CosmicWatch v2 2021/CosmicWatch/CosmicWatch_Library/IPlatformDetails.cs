using System;
using System.Collections.Generic;
using System.Text;

namespace CosmicWatch_Library
{
    public interface IPlatformDetails
    {
        String GetExternalStorageDir();
        String GetInternalStorageDir();
    }
}
