using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CosmicWatch.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OptionsPage : ContentPage
    {
        Action UpdateMainPage;
        public OptionsPage(Action UpdateMainPage)
        {
            InitializeComponent();
            InitializeUserSettings();

            this.UpdateMainPage += UpdateMainPage;
        }
        private void InitializeUserSettings()
        {
            TimeEntryFormatPicker.ItemsSource = Enum.GetValues(typeof(MainPage.NumberEntryFormat)).Cast<MainPage.NumberEntryFormat>().ToList();
            TimeEntryFormatPicker.SelectedIndex = TimeEntryFormatPicker.Items.IndexOf(UserSettings.NumberEntryFormat);
            RecordingRepeatCheckbox.IsChecked = UserSettings.IsRepeating;
            FakeRecordingCheckbox.IsChecked = UserSettings.IsFakeRecording;
        }

        private void OnTimeEntryFormatPickerChanged(object sender, EventArgs e)
        {
            UserSettings.NumberEntryFormat = TimeEntryFormatPicker.Items[TimeEntryFormatPicker.SelectedIndex];
        }

        private void OnRecordingRepeatCheckboxChanged(object sender, CheckedChangedEventArgs e)
        {
            UserSettings.IsRepeating = RecordingRepeatCheckbox.IsChecked;
        }

        private void OnRefreshMainPageButton(object sender, EventArgs e)
        {
            UpdateMainPage?.Invoke();
        }

        private void OnFakeRecordingCheckboxChanged(object sender, CheckedChangedEventArgs e)
        {
            UserSettings.IsFakeRecording = FakeRecordingCheckbox.IsChecked;
        }
    }
}