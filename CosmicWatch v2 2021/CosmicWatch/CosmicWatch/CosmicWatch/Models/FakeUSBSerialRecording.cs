using CosmicWatch_Library;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CosmicWatch.Models
{
    public class FakeUSBSerialRecording : IUSBSerialConnection
    {
        //Communication functions
        //Allows the connection to communicate back its data and status in the method preferred by the implementer.
        Action<String> UpdateData;
        Action<String> UpdateStatus;
        Action<IList> UpdateSupportedDevices;

        //Settings variables
        uint MaxReadLength;
        bool isRecording;

        //Recording Variables
        int MuonCounter;
        long ArduinoTime;
        double Amplitude;
        double SiPM;
        double DeadTime;

        //RNG Generator
        Random random;

        //Strings

        String FakeMachineName = "Fake Machine";

        public void Initialize(Action<String> UpdateDataOutput, Action<String> UpdateStatusMessage, Action<IList> UpdateSupportedDevices, uint MaxReadLength)
        {
            this.UpdateData = UpdateDataOutput;
            this.UpdateStatus = UpdateStatusMessage;
            this.UpdateSupportedDevices = UpdateSupportedDevices;
            this.MaxReadLength = MaxReadLength;

            random = new Random();

            UpdateSupportedDevices(new List<String> { FakeMachineName });
        }

        public async Task<bool> Connect(int ConnectIndex)
        {
            return true;
        }

        private String NextRecord()
        {
            MuonCounter += 1;
            Amplitude = random.NextDouble() * 2;
            SiPM = random.NextDouble();
            DeadTime = random.NextDouble() * 100;

            //Currently data is space-separated. This is for when it's comma-separated.
            //return $"{MuonCounter}, {ArduinoTime}, {Amplitude}, {SiPM}, {DeadTime} \n";
            return $"{MuonCounter} {ArduinoTime} {Amplitude} {SiPM} {DeadTime} \n";
        }

        public async Task RunRecording()
        {
            isRecording = true;
            MuonCounter = 0;
            ArduinoTime = 0;
            Amplitude = 0;
            SiPM = 0;
            DeadTime = 0;
            while (isRecording)
            {
                int NextTime = random.Next(0, 2000);
                ArduinoTime += NextTime;
                await Task.Delay(NextTime);
                UpdateData?.Invoke(NextRecord());
            }
        }

        public void StopRecording()
        {
            isRecording = false;
            return;
        }
    }
}
