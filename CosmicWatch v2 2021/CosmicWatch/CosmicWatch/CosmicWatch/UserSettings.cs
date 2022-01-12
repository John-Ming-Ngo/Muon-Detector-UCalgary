using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;

namespace CosmicWatch
{
    //Credits to the UserSettings sample class from https://docs.microsoft.com/en-us/xamarin/xamarin-forms/enterprise-application-patterns/configuration-management .

    /*
     This class is the static, saved between app-bootup user settings of this application.     
     */
    public static class UserSettings
    {
        static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }
        //[Main Page UI]
        public static String NumberEntryFormat
        {
            get => AppSettings.GetValueOrDefault(nameof(NumberEntryFormat), MainPage.NumberEntryFormat.Picker.ToString());
            set => AppSettings.AddOrUpdateValue(nameof(NumberEntryFormat), value);
        }

        public static bool IsRepeating
        {
            get => AppSettings.GetValueOrDefault(nameof(IsRepeating), false);
            set => AppSettings.AddOrUpdateValue(nameof(IsRepeating), value);
        }

        //[Main Page Model]
        public static bool IsFakeRecording
        {
            get => AppSettings.GetValueOrDefault(nameof(IsFakeRecording), true);
            set => AppSettings.AddOrUpdateValue(nameof(IsFakeRecording), value);
        }

        //[Saving to and Reading From Folder]
        public static string SaveDataFolder
        {
            get => AppSettings.GetValueOrDefault(nameof(SaveDataFolder), "CosmicWatchDetectorData");
            set => AppSettings.AddOrUpdateValue(nameof(SaveDataFolder), value);
        }

        public static string UploadDataWebsite
        {
            get => AppSettings.GetValueOrDefault(nameof(UploadDataWebsite), "http://localhost:8888/index.html");
            set => AppSettings.AddOrUpdateValue(nameof(UploadDataWebsite), value);
        }

        public static string UploadDataKey
        {
            get => AppSettings.GetValueOrDefault(nameof(UploadDataKey), "8kBx==!h");
            set => AppSettings.AddOrUpdateValue(nameof(UploadDataKey), value);
        }

        public static void ClearAllData()
        {
            AppSettings.Clear();
        }
    }
}