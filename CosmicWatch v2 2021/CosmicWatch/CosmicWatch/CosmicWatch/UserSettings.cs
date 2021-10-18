using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;

namespace CosmicWatch
{
    /// <summary>   
    /// This is the Settings static class that can be used in your Core solution or in any   
    /// of your client applications. All settings are laid out the same exact way with getters   
    /// and setters.     
    /// </summary>   
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

        public static void ClearAllData()
        {
            AppSettings.Clear();
        }
    }
}