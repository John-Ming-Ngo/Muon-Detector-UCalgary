using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CosmicWatch_Library
{
    public interface IUSBSerialConnection
    {
        void Initialize(Action<String> UpdateData, Action<String> UpdateStatus, Action<IList> UpdateSupportedDevices, uint MaxReadLength);
        
        Task<bool> Connect(int ConnectIndex);

        Task RunRecording();

        void StopRecording();

    }
}
