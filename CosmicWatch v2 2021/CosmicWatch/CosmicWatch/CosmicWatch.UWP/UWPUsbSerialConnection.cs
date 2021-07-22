using System;
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
using Xamarin.Forms;

//Current problem: Data builds up in a MASSIVE backlog. That's not what I want.
//I could dispose of and reinstate the serial port every time...
//That honestly sucks though. We should be able to do better, right?

//New problem: So, I can dispose and reinstate everytime... BUT THIS LEADS TO AN UNNATURALLY SLOW START AND SCREWS THE TIMER.
//THIS IS NOT WHAT I WANT.

[assembly: Dependency(typeof(UWPUsbSerialConnection))]
namespace CosmicWatch.UWP
{
    public class UWPUsbSerialConnection : IUSBSerialConnection
    {
        //Communication functions
        //Allows the connection to communicate back its data and status in the method preferred by the implementer.
        Action<String> UpdateData;
        Action<String> UpdateStatus;

        //Serial Connection and Reader attributes.
        //This is what this class was built to wrap.
        private String DeviceID;
        private SerialDevice SerialPort;
        private DataReader DataInputStream;

        //Status variables
        private bool IsRecording;

        //Settings variables
        uint MaxReadLength;

        public void Initialize(Action<String>  UpdateDataOutput, Action<String> UpdateStatusMessage, uint MaxReadLength = 32)
        {
            this.UpdateData = UpdateDataOutput;
            this.UpdateStatus = UpdateStatusMessage;
            this.MaxReadLength = MaxReadLength;
        }

        private void SetSerialSettings()
        {
            if (SerialPort == null) { return; }

            SerialPort.BreakSignalState = true;

            SerialPort.WriteTimeout = TimeSpan.FromMilliseconds(1000);
            SerialPort.ReadTimeout = TimeSpan.FromMilliseconds(1000);

            SerialPort.BaudRate = 9600;
            SerialPort.Parity = SerialParity.None;
            SerialPort.StopBits = SerialStopBitCount.One;
            SerialPort.DataBits = 8;
        }

        private async Task ConnectionStart(String ConnectionID)
        {
            SerialPort = await SerialDevice.FromIdAsync(ConnectionID);
            SetSerialSettings();
            if (SerialPort != null)
            {
                DataInputStream = new DataReader(SerialPort.InputStream);
            }
        }

        public async Task<bool> Connect()
        {
            DeviceInformationCollection serialDeviceInfos = await DeviceInformation.FindAllAsync(SerialDevice.GetDeviceSelector());

            foreach (DeviceInformation serialDeviceInfo in serialDeviceInfos)
            {
                if (DeviceID == serialDeviceInfo.Id) { return true; } //Trying to reinstantiate something already instantiated.
                
                try
                {
                    DeviceID = serialDeviceInfo.Id;
                    ConnectionStart(DeviceID);
                    return true;
                }
                catch (Exception e)
                {
                    UpdateStatus?.Invoke(e.Message);
                }
            }
            return false;
        }

        public async void RunRecording()
        {
            //if (DeviceID != null) { await ConnectionStart(DeviceID); }

            SerialPort.BreakSignalState = false;
            //IsRecording = true;
            
            await Task.Run(async () =>
            {
                uint bytesToRead;
                while (!SerialPort.BreakSignalState)//IsRecording)
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
            SerialPort.BreakSignalState = true;
            //IsRecording = false;
            //SerialPort?.Dispose();
            //DataInputStream?.Dispose();
        }
    }
}
