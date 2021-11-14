using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;

namespace CosmicWatch
{
    //Credits to the UserSettings sample class from https://docs.microsoft.com/en-us/xamarin/xamarin-forms/enterprise-application-patterns/configuration-management .
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

        public static void ClearAllData()
        {
            AppSettings.Clear();
        }
    }
}