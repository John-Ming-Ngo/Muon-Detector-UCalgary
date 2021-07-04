using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CosmicWatch_Library
{
    public interface IUSBSerialConnection
    {
        void Initialize(Action<String> UpdateData, Action<String> UpdateStatus, uint MaxReadLength);
        
        Task<bool> Connect();

        void RunRecording();

        void StopRecording();

    }
}
