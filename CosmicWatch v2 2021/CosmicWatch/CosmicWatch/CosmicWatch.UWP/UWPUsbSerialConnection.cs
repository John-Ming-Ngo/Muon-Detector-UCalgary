using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using CosmicWatch.UWP;
using CosmicWatch_Library;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Xamarin.Forms;

//Current problem: Data builds up in a MASSIVE backlog. That's not what I want.
//I could dispose of and reinstate the serial port every time...
//That honestly sucks though. We should be able to do better, right?

//New problem: So, I can dispose and reinstate everytime... BUT THIS LEADS TO AN UNNATURALLY SLOW START AND SCREWS THE TIMER.
//THIS IS NOT WHAT I WANT.
//TODO:
//https://social.msdn.microsoft.com/Forums/sqlserver/en-US/21afffb8-ca62-41cc-a38c-4e8303657152/uwpdetect-usb-drive-insert?forum=wpdevelop
//https://docs.microsoft.com/en-us/previous-versions/windows/apps/hh967756(v=win.10)?redirectedfrom=MSDN

[assembly: Dependency(typeof(UWPUsbSerialConnection))]
namespace CosmicWatch.UWP
{
    public class UWPUsbSerialConnection : IUSBSerialConnection
    {
        //Communication functions
        //Allows the connection to communicate back its data and status in the method preferred by the implementer.
        Action<String> UpdateData;
        Action<String> UpdateStatus;
        Action<IList> UpdateSupportedDevices;

        //Serial Connection and Reader attributes.
        //This is what this class was built to wrap.
        private String DeviceID;
        private SerialDevice SerialPort;
        private DataReader DataInputStream;
        private DeviceWatcher WatchDevices;

        //Status variables
        private bool IsRecording;

        //Settings variables
        uint MaxReadLength;
        int ReadTimeOutMs = 1000;
        int WriteTimeOutMs = 1000;

        //Default Serial Port Settings

        public void Initialize(Action<String>  UpdateDataOutput, Action<String> UpdateStatusMessage, Action<IList> UpdateSupportedDevices, uint MaxReadLength)
        {
            this.UpdateData = UpdateDataOutput;
            this.UpdateStatus = UpdateStatusMessage;
            this.UpdateSupportedDevices = UpdateSupportedDevices;
            this.MaxReadLength = MaxReadLength;

            try
            {
                WatchDevices = DeviceInformation.CreateWatcher();
                WatchDevices.Added += WatchDevicesAdded;
                WatchDevices.Removed += WatchDevicesRemoved;
                WatchDevices.Updated += WatchDevicesUpdated;
                WatchDevices.EnumerationCompleted += WatchDevicesEnumerationCompleted;
                WatchDevices.Stopped += WatchDevicesStopped;
                WatchDevices.Start();
            }
            catch (ArgumentException)
            {
                UpdateStatus("Failed to create device watcher.");
            }
        }

        private void SetSerialSettings()
        {
            //SerialPort.BreakSignalState = true;

            SerialPort.WriteTimeout = TimeSpan.FromMilliseconds(ReadTimeOutMs);
            SerialPort.ReadTimeout = TimeSpan.FromMilliseconds(WriteTimeOutMs);

            SerialPort.BaudRate = 9600;
            SerialPort.Parity = SerialParity.None;
            SerialPort.StopBits = SerialStopBitCount.One;
            SerialPort.DataBits = 8;
        }

        private async Task ConnectionStart(String ConnectionID)
        {
            SerialPort = await SerialDevice.FromIdAsync(ConnectionID);
            
            if (SerialPort != null)
            {
                SetSerialSettings();
                DataInputStream = new DataReader(SerialPort.InputStream);
            }
        }

        public async Task<bool> Connect(int Selection)
        {
            DeviceInformationCollection serialDeviceInfos = await DeviceInformation.FindAllAsync(SerialDevice.GetDeviceSelector());

            DeviceInformation serialDeviceInfo = serialDeviceInfos.ElementAt(Selection);
            if (DeviceID == serialDeviceInfo.Id) { return true; } //Trying to reinstantiate something already instantiated.
            
            try
            {
                DeviceID = serialDeviceInfo.Id;
                //ConnectionStart(DeviceID);
                return true;
            }
            catch (Exception e)
            {
                UpdateStatus?.Invoke(e.Message);
            }
            return false;
        }

        public async Task RunRecording()
        {
            //Data Input Stream, the datareader, is slow.

            if (DeviceID != null) { await ConnectionStart(DeviceID); }

            //SerialPort.BreakSignalState = false;
            IsRecording = true;

            await Task.Run(async () =>
            {
                uint bytesToRead;
                while (IsRecording)//!SerialPort.BreakSignalState)//
                {
                    try
                    {
                        bytesToRead = await DataInputStream.LoadAsync(MaxReadLength);
                        UpdateData?.Invoke(DataInputStream.ReadString(bytesToRead));
                    }
                    catch (Exception)
                    {
                        IsRecording = false;
                    }
                }
            });
        }

        public void StopRecording()
        {
            //SerialPort.BreakSignalState = true;
            IsRecording = false;
            SerialPort?.Dispose();
            DataInputStream?.Dispose();
        }

        //Methods to execute when a device has been detected added or removed.
        async void UpdateDeviceChoices()
        {
            DeviceInformationCollection serialDeviceInfos = await DeviceInformation.FindAllAsync(SerialDevice.GetDeviceSelector());
            List<String> SelectionChoices = new List<String>();
            foreach (DeviceInformation serialDeviceInfo in serialDeviceInfos)
            {
                SelectionChoices.Add(serialDeviceInfo.Name);
            }
            UpdateSupportedDevices(SelectionChoices);
        }
        async void StopWatcher(object sender, RoutedEventArgs eventArgs)
        {
            try
            {
                if (WatchDevices.Status == Windows.Devices.Enumeration.DeviceWatcherStatus.Stopped)
                {
                    return;
                }
                else
                {
                    WatchDevices.Stop();
                }
            }
            catch (ArgumentException)
            {
                
            }
        }

        async void WatchDevicesAdded(DeviceWatcher sender, DeviceInformation deviceInterface)
        {
            //UpdateStatus("Attempted to add devices.");
            /**
            DeviceInformationCollection serialDeviceInfos = await DeviceInformation.FindAllAsync(SerialDevice.GetDeviceSelector());
            List<String> SelectionChoices = new List<String>();
            foreach (DeviceInformation serialDeviceInfo in serialDeviceInfos)
            {
                SelectionChoices.Add(serialDeviceInfo.Name);
            }
            UpdateSupportedDevices(SelectionChoices);
            */
        }

        async void WatchDevicesUpdated(DeviceWatcher sender, DeviceInformationUpdate devUpdate)
        {
            //UpdateStatus("Attempted to update devices.");

            DeviceInformationCollection serialDeviceInfos = await DeviceInformation.FindAllAsync(SerialDevice.GetDeviceSelector());
            List<String> SelectionChoices = new List<String>();
            foreach (DeviceInformation serialDeviceInfo in serialDeviceInfos)
            {
                SelectionChoices.Add(serialDeviceInfo.Name);
            }
            UpdateSupportedDevices(SelectionChoices);
        }

        async void WatchDevicesRemoved(DeviceWatcher sender, DeviceInformationUpdate devUpdate)
        {
            //UpdateStatus("Attempted to remove devices.");
            /**
            DeviceInformationCollection serialDeviceInfos = await DeviceInformation.FindAllAsync(SerialDevice.GetDeviceSelector());
            List<String> SelectionChoices = new List<String>();
            foreach (DeviceInformation serialDeviceInfo in serialDeviceInfos)
            {
                SelectionChoices.Add(serialDeviceInfo.Name);
            }
            UpdateSupportedDevices(SelectionChoices);
            */

        }

        async void WatchDevicesEnumerationCompleted(DeviceWatcher sender, object args)
        {
            DeviceInformationCollection serialDeviceInfos = await DeviceInformation.FindAllAsync(SerialDevice.GetDeviceSelector());
            List<String> SelectionChoices = new List<String>();
            foreach (DeviceInformation serialDeviceInfo in serialDeviceInfos)
            {
                SelectionChoices.Add(serialDeviceInfo.Name);
            }
            UpdateSupportedDevices(SelectionChoices);
        }

        async void WatchDevicesStopped(DeviceWatcher sender, object args)
        {

        }

    }
}
