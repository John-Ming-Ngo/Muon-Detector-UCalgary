using System;
using System.Timers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

using Xamarin.Forms;
using Xamarin.Essentials;

using CosmicWatch.Views;
using CosmicWatch.ViewModels;

using CosmicWatch_Library;



namespace CosmicWatch
{
    /*
    Code-behind for the main screen
    Viewmodel is MainPageModel

    This page handles all the UI Logic for the Main Page
    This means any changes or functionality which comes about due to the UI
    Ex. Functions to handle screen transitions, button presses or what can be displayed.

    Organization:
        1. Data and References
        2. By location on the screen (Top first, bottom last)

    Any data manipulation is sent to the Viewmodel to handle and return to here.

    ConfigFileStuffToDO:
    https://www.c-sharpcorner.com/article/learn-about-user-settings-in-xamarin-forms/
    https://docs.microsoft.com/en-us/xamarin/xamarin-forms/enterprise-application-patterns/configuration-management
    https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/application-class#properties-dictionary
     */
    public partial class MainPage : ContentPage
    {
        
        //[Constants]
        const int MillsPerSec = 1000;
        const int SecPerMin = 60;
        const int MinPerHour = 60;

        //[Strings]
        String RecordBoxError = "Please enter numbers only! Recording period (s)";
        String RecordBoxPrompt = "Recording period (s):";

        String RepeatBoxError = "Please enter numbers only! Repeat delay (s)";

        String BeginRecordNonNumeric = "Please enter numbers only!";
        String BeginRecordZeroOrLess = "I require some amount of time to record for!";
        String BeginRecordNoDevice = "No Device Connected!";
        String BeginRecordButtonSuccess = "Recording...";

        String DelayRecordButtonSuccess = "Waiting Until Next Recording...";
        String EndRecordButtonSuccess = "Start Recording.";

        String ConnectedTrue = "Connected";
        String ConnectedFalse = "Not Connected";

        //[Viewmodel]
        private MainPageModel PageModel {set; get;}

        //[Constructor]
        public MainPage() {
            
            //Initializations
            InitializeComponent();
            InitializeUI();

            //Establish the Page Model
            PageModel = new MainPageModel(UpdateMuonDisplay, UpdateMuonsPerMinuteDisplay, UpdateConnectedDisplay, UpdateTimeDisplay, UpdateElapsedDisplay, UpdateDeviceList, UpdateStatusMessage, EndRecording);
        }
        //[Page initialization and refreshing]
        private void InitializeUI()
        {
            InitializeUserSettings();

            InitiateTimeInputFormat();
            InitiateTimePickers();
            InitiateDelayMenu();
        }


        //[Menu Bar Buttons]
        private void OnOptions(object sender, EventArgs e) {
            InitializeUI();
            Navigation.PushAsync(new OptionsPage(InitializeUI));
        }
        private void OnDataAnalysis(object sender, EventArgs e) {
            Navigation.PushAsync(new DisplayAnalysisPage());
        }
        private void OnUsageGuide(object sender, EventArgs e)
        {
            Navigation.PushAsync(new UsageGuidePage());
        }
        private void OnCredits(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CreditsPage());
        }
        //[Center Options  - Display]
        private void UpdateTimeDisplay(DateTime time) {
            Device.BeginInvokeOnMainThread(() => timeDisplay.Text = $"{time:MM/dd/yyyy HH:mm:ss}");
        }
        private void UpdateElapsedDisplay(double elapsedMilleseconds)
        {
            Device.BeginInvokeOnMainThread(() => elapsedDisplay.Text = $"Elapsed Time: {elapsedMilleseconds/MillsPerSec : 0.0} (s)");
        }
        private void UpdateMuonDisplay(long count) {
            Device.BeginInvokeOnMainThread(() => muonCountsDisplay.Text = $"{count}");
        }
        private void UpdateMuonsPerMinuteDisplay(double rate) {
            Device.BeginInvokeOnMainThread(() => muonsPerMinuteDisplay.Text = $"{rate:0.00}");
        }

        //[Center Options: Recording Length]
        public enum NumberEntryFormat
        {
            TextEntry,
            Picker
        }

        private NumberEntryFormat EntryFormat;

        private void InitializeUserSettings()
        {
            if (!Enum.TryParse(UserSettings.NumberEntryFormat, out EntryFormat))
            {
                EntryFormat = NumberEntryFormat.TextEntry;
            }
            isDelayRepeating = UserSettings.IsRepeating;
        }

        private void InitiateTimeInputFormat()
        {
            EntryTimeLayout.IsVisible = EntryFormat == NumberEntryFormat.TextEntry;
            PickerTimeLayout.IsVisible = EntryFormat == NumberEntryFormat.Picker;

            EntryTimeRepeatLayout.IsVisible = EntryFormat == NumberEntryFormat.TextEntry;
            PickerTimeRepeatLayout.IsVisible = EntryFormat == NumberEntryFormat.Picker;
        }

        private void InitiateTimePickers()
        {
            //Time To Record For Pickers - Hours, Minutes, Seconds
            recordingTimeHoursPicker.ItemsSource = 
                Enumerable.Range(0, 24).ToList();
            recordingTimeMinutesPicker.ItemsSource = Enumerable.Range(0, 60).ToList();
            recordingTimeSecondsPicker.ItemsSource = Enumerable.Range(0, 60).ToList();

            recordingTimeHoursPicker.SelectedIndex = 0;
            recordingTimeMinutesPicker.SelectedIndex = 0;
            recordingTimeSecondsPicker.SelectedIndex = 0;

            //Time Delay Before Repeating Pickers - Hours, Minutes, Seconds
            recordingTimeHoursRepeatPicker.ItemsSource = Enumerable.Range(0, 24).ToList();
            recordingTimeMinutesRepeatPicker.ItemsSource = Enumerable.Range(0, 60).ToList();
            recordingTimeSecondsRepeatPicker.ItemsSource = Enumerable.Range(0, 60).ToList();

            recordingTimeHoursRepeatPicker.SelectedIndex = 0;
            recordingTimeMinutesRepeatPicker.SelectedIndex = 0;
            recordingTimeSecondsRepeatPicker.SelectedIndex = 0;
        }

        private void OnRecordingLengthEntry(object sender, EventArgs e)
        {
            Entry recordLengthBox = (Entry)sender;
            String recordLength = recordLengthBox.Text;
            //Verify entered text is numeric only, if keyboard cannot be selected.
            //Error checking only really needed for computers at the current moment.
            bool isNumeric = double.TryParse(recordLength, out _);
            if (!isNumeric) 
            {
                //Delete the last character that was entered.
                recordLengthBox.Text = recordLength.Substring(0, Math.Max(recordLength.Length - 1, 0));
                UpdateStatusMessage(RecordBoxError);
            }
            else
            {
                recordLengthBox.Placeholder = RecordBoxPrompt;
            }
        }

        //[Center Options - Buttons]

        private long ParseEntryTimeSelection()
        {
            long Seconds = 0;

            bool isNumeric = long.TryParse(recordingTimeEntry.Text, out Seconds);
            if (!isNumeric)
            {
                UpdateStatusMessage(BeginRecordNonNumeric);
            }

            return Seconds;
        }

        private long ParsePickerTimeSelection()
        {
            _ = long.TryParse(recordingTimeHoursPicker.Items[recordingTimeHoursPicker.SelectedIndex], out long PickerHours);
            _ = long.TryParse(recordingTimeMinutesPicker.Items[recordingTimeMinutesPicker.SelectedIndex], out long PickerMinutes);
            _ = long.TryParse(recordingTimeSecondsPicker.Items[recordingTimeSecondsPicker.SelectedIndex], out long PickerSeconds);
            long Seconds = PickerHours * MinPerHour * SecPerMin + PickerMinutes * SecPerMin + PickerSeconds;
            return Seconds;
        }
        
        private long GetRecordTime()
        {
            long Seconds = 0;

            switch (EntryFormat)
            {
                case NumberEntryFormat.Picker:
                    Seconds = ParsePickerTimeSelection();
                    break;
                case NumberEntryFormat.TextEntry:
                    Seconds = ParseEntryTimeSelection();
                    break;
            }

            return Seconds;
        }

        private void BeginRecording()
        {
            RestartRecording = null;

            long Seconds = GetRecordTime();

            if (Seconds <= 0)
            {
                UpdateStatusMessage(BeginRecordZeroOrLess);
            }
            else if (!PageModel.DeviceConnected)
            {
                UpdateStatusMessage(BeginRecordNoDevice);
            }
            else
            {
                UpdateStatusMessage(String.Empty);
                recordButton.Text = BeginRecordButtonSuccess;

                //Start the Recording
                _ = PageModel.Recording(Seconds);
            }
        }

        //[Center - Repeat Delay Options]

        private bool isDelayRepeating;

        private void InitiateDelayMenu()
        {
            RepetitionDelayRow.IsVisible = isDelayRepeating;
        }

        public void OnRepeatLengthEntry(object sender, EventArgs e)
        {
            Entry recordLengthBox = (Entry)sender;
            String recordLength = recordingTimeRepeatEntry.Text;
            //Verify entered text is numeric only, if keyboard cannot be selected.
            //Error checking only really needed for computers at the current moment.
            bool isNumeric = double.TryParse(recordLength, out _);
            if (!isNumeric)
            {
                recordingTimeRepeatEntry.Text = recordLength.Substring(0, Math.Max((recordLength.Length - 1), 0));
                UpdateStatusMessage(RepeatBoxError);
            }
            else
            {
                recordingTimeRepeatEntry.Placeholder = RecordBoxPrompt;
            }
        }

        private long ParseEntryTimeRepeatSelection()
        {
            long Seconds = 0;

            bool isNumeric = long.TryParse(recordingTimeRepeatEntry.Text, out Seconds);
            if (!isNumeric)
            {
                UpdateStatusMessage(BeginRecordNonNumeric);
            }

            return Seconds;
        }

        private long ParsePickerTimeRepeatSelection()
        {
            _ = long.TryParse(recordingTimeHoursRepeatPicker.Items[recordingTimeHoursRepeatPicker.SelectedIndex], out long PickerHours);
            _ = long.TryParse(recordingTimeMinutesRepeatPicker.Items[recordingTimeMinutesRepeatPicker.SelectedIndex], out long PickerMinutes);
            _ = long.TryParse(recordingTimeSecondsRepeatPicker.Items[recordingTimeSecondsRepeatPicker.SelectedIndex], out long PickerSeconds);
            long Seconds = (PickerHours * MinPerHour * SecPerMin) + (PickerMinutes * SecPerMin) + PickerSeconds;
            return Seconds;
        }

        private long GetRepeatDelay()
        {
            long Seconds = 0;

            switch (EntryFormat)
            {
                case NumberEntryFormat.Picker:
                    Seconds = ParsePickerTimeRepeatSelection();
                    break;
                case NumberEntryFormat.TextEntry:
                    Seconds = ParseEntryTimeRepeatSelection();
                    break;
            }
            return Seconds;
        }

        private DelayedEvent RestartRecording;

        private void BeginDelayedRecording()
        {
            RestartRecording = new DelayedEvent(GetRepeatDelay() * MillsPerSec, (() =>
            {
                if (!PageModel.recording) BeginRecording();
            }));
        }

        //[Center - End Recording]
        private void EndRecording()
        {
            recordingTimeEntry.IsEnabled = true;
            recordButton.Text = EndRecordButtonSuccess;
            
            if (isDelayRepeating)
            {
                recordButton.Text = DelayRecordButtonSuccess;
                BeginDelayedRecording();
            }
        }

        private void EndRecordingEarly()
        {
            PageModel.StopRecordingEarly();
        }

        private void OnRecord(object sender, EventArgs e)
        {
            if (RestartRecording != null)
            {
                RestartRecording.Terminate();
                RestartRecording = null;

                recordButton.Text = EndRecordButtonSuccess;
            }
            else if (!PageModel.recording)
            {
                BeginRecording();
            }
            else
            {
                EndRecordingEarly();
            }
        }

        //[Center - Status Message]
        public void UpdateStatusMessage(String message)
        {
            Device.BeginInvokeOnMainThread(() => statusDisplay.Text = message);
        }

        //[Bottom Bar - Picker]
        private void UpdateDeviceList(IList SupportedDevices) {
            Device.BeginInvokeOnMainThread(() => supportedDevices.ItemsSource = SupportedDevices);
        }
        //[Bottom Bar - Connection Status label.]
        private void UpdateConnectedDisplay(bool isConnected) {
            Device.BeginInvokeOnMainThread(() => ConnectionLabel.Text = isConnected ? ConnectedTrue : ConnectedFalse);
        }
        //[Bottom Bar - Connection Button]
        private async void OnConnect(object sender, EventArgs e) {
            await PageModel.Connect(supportedDevices.SelectedIndex);
        }
    }
}
