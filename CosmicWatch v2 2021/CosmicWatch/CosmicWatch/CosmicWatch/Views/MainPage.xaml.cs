using System;
using System.Timers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using CosmicWatch.Views;
using CosmicWatch.ViewModels;

using CosmicWatch_Library;
using Xamarin.Essentials;

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
     */
    public partial class MainPage : ContentPage
    {
        //[Viewmodel]
        private MainPageModel PageModel {set; get;}

        //[Constructor]
        public MainPage() {
            InitializeComponent();

            PageModel = new MainPageModel(UpdateMuonDisplay, UpdateMuonsPerMinuteDisplay, UpdateConnectedDisplay, UpdateTimeDisplay, UpdateElapsedDisplay, UpdateDeviceList, UpdateStatusMessage, EndRecording);
        }
        //[Menu Bar Buttons]
        private void OnOptions(object sender, EventArgs e) {
            Navigation.PushAsync(new OptionsPage());
        }
        private void OnDataAnalysis(object sender, EventArgs e) {
            Navigation.PushAsync(new DisplayAnalysisPage());
        }
        //[Center Options  - Display]
        private void UpdateTimeDisplay(DateTime time) {
            Device.BeginInvokeOnMainThread(() => timeDisplay.Text = $"{time:MM/dd/yyyy HH:mm:ss}");
        }
        private void UpdateElapsedDisplay(double elapsedMilleseconds)
        {
            Device.BeginInvokeOnMainThread(() => elapsedDisplay.Text = $"Elapsed Time: {elapsedMilleseconds/1000 : 0.0} (s)");
        }
        private void UpdateMuonDisplay(long count) {
            Device.BeginInvokeOnMainThread(() => muonCountsDisplay.Text = $"{count}");
        }
        private void UpdateMuonsPerMinuteDisplay(double rate) {
            Device.BeginInvokeOnMainThread(() => muonsPerMinuteDisplay.Text = $"{rate:0.00}");
        }
        //[Center Options: Recording Length]
        public void OnRecordingLengthEntry(object sender, EventArgs e)
        {
            Entry recordLengthBox = (Entry)sender;
            String recordLength = recordLengthBox.Text;
            //Verify entered text is numeric only, if keyboard cannot be selected.
            //Error checking only really needed for computers at the current moment.
            bool isNumeric = double.TryParse(recordLength, out _);
            if (!isNumeric) 
            { 
                recordLengthBox.Text = "";
                recordLengthBox.Placeholder = "Please enter numbers only! Recording period (s)";
            }
            else
            {
                recordLengthBox.Placeholder = "Recording period (s):";
            }
        }
        //[Center Options - Buttons]

        private void BeginRecording()
        {
            bool isNumeric = long.TryParse(recordingTimeEntry.Text, out long Seconds);
            if (!isNumeric)
            {
                UpdateStatusMessage("Please enter numbers only!");
            }
            else if (Seconds <= 0)
            {
                UpdateStatusMessage("I require some amount of time to record for!");
            }
            else if (!PageModel.DeviceConnected)
            {
                UpdateStatusMessage("No Device Connected!");
            }
            else
            {
                UpdateStatusMessage("");
                recordingTimeEntry.IsEnabled = false;
                recordButton.Text = "Recording...";

                //Start the Recording
                PageModel.Recording(Seconds);
            }
        }

        private void EndRecording()
        {
            recordingTimeEntry.IsEnabled = true;
            recordButton.Text = "Start Recording.";
        }

        private void EndRecordingEarly()
        {
            PageModel.StopRecordingEarly();
        }

        private void OnRecord(object sender, EventArgs e) {
            if (!PageModel.recording)
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
        private void UpdateDeviceList(List<String> SupportedDevices) {
            Device.BeginInvokeOnMainThread(() => supportedDevices.ItemsSource = SupportedDevices);
        }
        private void OnDeviceChange(object sender, EventArgs e) {
            Picker picker = (Picker)sender;
            int index = picker.SelectedIndex;
            if (index != -1)
            {
                //Do something about the new picker.
                PageModel.ChangeDevice(index);
            }
        }
        //[Bottom Bar - Connection Status label.]
        private void UpdateConnectedDisplay(bool isConnected) {
            Device.BeginInvokeOnMainThread(() => ConnectionLabel.Text = isConnected ? "Connected" : "Not Connected");
        }
        //[Bottom Bar - Connection Button]
        private async void OnConnect(object sender, EventArgs e) {
            await PageModel.Connect();
        }
    }
}
