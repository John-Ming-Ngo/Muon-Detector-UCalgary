using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Forms;
using CosmicWatch.Models;

using CosmicWatch_Library;
using Xamarin.Essentials;
using System.Collections;

namespace CosmicWatch.ViewModels
{

    /*
    Purpose: 
        Main view model of the main page.
        Handles the data manipulation for the main page.
   
    Organization of this page:
        1. Data and References
        2. By location on the screen (Top first, bottom last)
    
    On Interactions with the Main Page:
        Takes arguments from the main page:
            Through the contructor
            Via public functions
        Returns data to the main page:
            Via display update functions.
        
        Intended Pattern of Data Transfer:
            Main Page to Main Page Model:
                What to do.
            Main Page Model to Main Page:
                What to display.
        
        Anything else is bad and needs to be refactored.

    On Interactions with Utility Functions from CosmicWatch Library:
        IUSBSerialConnection:
            Purpose: Handles connections and data transfer from a USB Serial Connection.
            To Model: Starting and Ending the serial communication, as well as the function to pass data back with and recording settings.
            From Model: USB Serial Communication Data (receive)

    On Models this Viewmodel Interacts with:
        DetectionRecord:
            Purpose: Records and saves the detection data.
            To Model: String detection data, initiation and closing/saving of the record.
            From Model: Current processed values of interest from the record.
    */
    public class MainPageModel
    {
        //[Strings]
        String RecordNameSuffix = "MuonRecordingFile-";
        String RecordTimeFormat = "yyyy-MM-ddTHH-mm-ss";
        String RecordFileExtension = ".csv";

        //[Displays/Data Exit Functions]
        public Action<long> UpdateMuonDisplay;
        public Action<double> UpdateMuonsPerMinuteDisplay;
        public Action<bool> UpdateConnectedDisplay;
        public Action<DateTime> UpdateTimeDisplay;
        public Action<double> UpdateElapsedDisplay;
        //public Action<List<String>> UpdateSupportedDevices;
        public Action<IList> UpdateSupportedDevices;
        public Action<String> UpdateStatusMessage;

        public Action EndRecording;

        //[Models]
        IUSBSerialConnection USBSerialConnection;
        DetectionRecord record;

        //[Constructor]
        public MainPageModel(Action<long> UpdateMuonDisplay, Action<double> UpdateMuonsPerMinuteDisplay, Action<bool> UpdateConnectedDisplay, Action<DateTime> UpdateTimeDisplay, Action<double> UpdateElapsedDisplay, Action<IList> UpdateSupportedDevices, Action<String> UpdateStatusMessage, Action EndRecording)
        {
            //Save the given display functions to local memory for later use.
            this.UpdateMuonDisplay += UpdateMuonDisplay;
            this.UpdateMuonsPerMinuteDisplay += UpdateMuonsPerMinuteDisplay;
            this.UpdateConnectedDisplay += UpdateConnectedDisplay;
            this.UpdateTimeDisplay += UpdateTimeDisplay;
            this.UpdateElapsedDisplay += UpdateElapsedDisplay;
            this.UpdateSupportedDevices += UpdateSupportedDevices;
            this.UpdateStatusMessage += UpdateStatusMessage;
            this.EndRecording += EndRecording;
            
            //Initiate the platform appropriate implementation of the USB serial connection.
            USBSerialConnection = DependencyService.Get<IUSBSerialConnection>();
            USBSerialConnection.Initialize(ReceiveData, UpdateStatusMessage, UpdateSupportedDevices, 32);

            //Inititalize variables which need an unambiguous initial status.
            recording = false;

            //Debug stuff.
            //UpdateStatusMessage(DependencyService.Get<IPlatformDetails>().GetExternalStorageDir());
        }

        //[Navigation bar]

        //[Center]

        //Current Time Display Data as Attribute.
        private DateTime timeStamp;
        public DateTime TimeStamp { 
            get { return timeStamp; } 
            set {
                timeStamp = value;
                UpdateTimeDisplay?.Invoke(timeStamp);
            } 
        }

        //Current Muon Count Display Data as Attribute.
        private long muonCount;
        public long MuonCount { 
            get { return muonCount; }
            set {
                muonCount = value;
                UpdateMuonDisplay?.Invoke(muonCount);
            } 
        }

        //Method status variable: Whether or not there is an ongoing recording.
        public bool recording;

        //[Bottom bar]

        //Current Supprted Devices List Data as Attribute.
        private List<String> supportedDevices;
        public List<String> SupportedDevices {
            get { return supportedDevices; }
            set {
                supportedDevices = value;
                UpdateSupportedDevices?.Invoke(supportedDevices);
            } 
        }

        //Index of the list.
        private int DeviceIndex;

        //Current Device Connectivity Data as Attribute.
        private bool deviceConnected;
        public bool DeviceConnected
        {
            get { return deviceConnected; }
            set {
                deviceConnected = value;
                UpdateConnectedDisplay?.Invoke(deviceConnected);
            }
        }

        //[Methods: Navigation bar]

        //[Methods: Data Processing and Buffering]

        private void ReceiveData(String input)
        {
            record?.ReceiveData(input);
        }

        //[Methods: Center]

        public async Task Recording(long Time)
        {            
            //Prepare the data recording.
             String filename = RecordNameSuffix + DateTime.Now.ToString(RecordTimeFormat) + RecordFileExtension;
            
            //Variable to indicate recording start.
            recording = true;
            
            //Start the recording.
            USBSerialConnection.RunRecording();
            record = new DetectionRecord(filename);

            while (recording && (record.stopwatch.ElapsedMilliseconds < (Time * 1000)))
            {
                //Wait 25 milleseconds for data to accumulate.
                //Todo: Make this selectable/more dynamic? 25 milleseconds is rather arbitrary.
                //Also, is there a better way?
                await Task.Delay(25);
                //Update the time.
                UpdateTimeDisplay?.Invoke(DateTime.Now);
                UpdateElapsedDisplay?.Invoke(record.stopwatch.ElapsedMilliseconds);

                //Update our data with our new data records.
                MuonCount = record.EventCount;
                UpdateMuonsPerMinuteDisplay?.Invoke(record.EventsPerMinute);
            }
            recording = false;
            //Clean up finished recording.
            USBSerialConnection.StopRecording();
            record.Close();
            //End the recording.
            EndRecording?.Invoke();
        }
        public void StopRecordingEarly()
        {
            recording = false;
        }

        //[Methods: Bottom Bar]
        public void ChangeDevice(int supportedIndex) {
            DeviceIndex = supportedIndex;
        }

        public async Task Connect(int SelectedIndex) {
            if (SelectedIndex == -1)
            {
                UpdateStatusMessage?.Invoke("No Device Selected!");
                DeviceConnected = false;
                return;
            }
            DeviceConnected = await USBSerialConnection.Connect(SelectedIndex);
        }
    }
}

/*
 Legacy Code, used before.
 
        private GeolocationString geolocator;

            //geolocator = new GeolocationString();
            //geolocator.GetCurrentLocationName(UpdateStatusMessage);
            //UpdateStatusMessage(FileSystem.AppDataDirectory);
            //UpdateStatusMessage(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));

            UpdateStatusMessage(DependencyService.Get<IPlatformDetails>().GetExternalStorageDir());

        private async Task FakeRecording() {
            MuonCount = 0;
            recording = true;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (recording) {
                await Task.Run(() => {
                    System.Threading.Thread.Sleep(20);
                    });
                UpdateTimeDisplay?.Invoke(DateTime.Now);
                MuonCount += 1;
                Double MuonsPerMinute = (MuonCount / (stopwatch.ElapsedMilliseconds / 1000.0));
                UpdateMuonsPerMinuteDisplay?.Invoke(MuonsPerMinute);
            }
            stopwatch.Stop();
        }
 */

