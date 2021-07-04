using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;

namespace CosmicWatch_Library
{
    public class USBSerialManager
    {
        Action<String> OnDetect;
        private String CallBuffer;

        IUSBSerialConnection UsbSerialConnection;

        public USBSerialManager() {
            UsbSerialConnection = DependencyService.Get<IUSBSerialConnection>();
        }

        public bool TryConnect()
        {
            Task<bool> DeviceConnectAttempt = UsbSerialConnection.ConnectToDevice();
            return true;
            /*
            DeviceConnectAttempt.Wait();
            return DeviceConnectAttempt.Result;
            */
        }

        bool isRecording = false;

        public void beginRecording()
        {
            if (!isRecording)
            {
                isRecording = true;
                Timer timer = new Timer();
            }
        }

        public String ReadData()
        {
            Task<String> ReadAttempt = UsbSerialConnection.ReadData();
            ReadAttempt.Wait();
            return ReadAttempt.Result;
        }

        public void RegisterDetectedFunction(Action<String> OnDetect)
        {
            this.OnDetect += OnDetect;
            UsbSerialConnection.RegisterUpdateFunction(this.OnDetect);
        }

    }
}
