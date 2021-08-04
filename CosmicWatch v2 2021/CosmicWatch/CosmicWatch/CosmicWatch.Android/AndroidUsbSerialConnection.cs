using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Hardware.Usb;
using Android.Util;


using CosmicWatch.Droid;
using CosmicWatch_Library;
using Xamarin.Forms;

using Hoho.Android.UsbSerial.Driver;
using Hoho.Android.UsbSerial.Extensions;
using Hoho.Android.UsbSerial.Util;

[assembly: UsesFeature("android.hardware.usb.host")]
[assembly: Dependency(typeof(AndroidUsbSerialConnection))]

namespace CosmicWatch.Droid
{
    /*
     Purpose: 
        Implementation and wrapper of a USB Serial Communications Connection on the Android Platform.
        See IUSBSerialConnection in the CosmicWatch Library for more.
        
    Organization of this page:
        1. Data and References
        2. Purpose and Function.
        3. IUSBSerialConnection Interface Functions and Dependent Functions

    Credits:
        Credits to the contributers of the UsbSerialForAndroid library.
        Their library was utilized to implement most of the functionality here.
        Furthermore, their code examples explained how to implement this functionality.
        Direct snippets will be credited to them here.

        The library can be found here: 
            https://github.com/anotherlab/UsbSerialForAndroid

    Todo: 
     */

    //[Attributes]
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] { UsbManager.ActionUsbDeviceAttached, UsbManager.ActionUsbAccessoryDetached })]
    [MetaData(UsbManager.ActionUsbDeviceAttached, Resource = "@xml/device_filter")]
    
    public class AndroidUsbSerialConnection : IUSBSerialConnection
    {
        //[Return Communication Functions]
        Action<String> UpdateData;
        Action<String> UpdateStatus;

        //[Serial Connection Attributes]
        UsbManager UsbManager;
        SerialInputOutputManager SerialIOManager;

        //[Broadcast Receivers]
        UsbDeviceAttachedReceiver AttachedReceiver;
        UsbDeviceDetachedReceiver DetachedReceiver;

        //[Global variables]
        Context LocContext;

        //[Settings Attributes]
        uint MaxReadLength;

        //[Strings]
        String ConnectFailNoDriver = "No Driver!";
        String ConnectFailNoSerialDevice = "No serial device!";


        //[Pseudo-Constructor]
        public void Initialize(Action<String> UpdateDataOutput, Action<String> UpdateStatusMessage, uint MaxReadLength)
        {
            //Set the return functions and any possibly used attributes.
            this.UpdateData = UpdateDataOutput;
            this.UpdateStatus = UpdateStatusMessage;
            this.MaxReadLength = MaxReadLength;

            //Get important local variables
            LocContext = Android.App.Application.Context;
            UsbManager = (UsbManager)LocContext.GetSystemService(Context.UsbService);

            //Detect and ask for permission upon detecting a new USB connection.
            AttachedReceiver = new UsbDeviceAttachedReceiver(this, LocContext);
            LocContext.RegisterReceiver(AttachedReceiver, new IntentFilter(UsbManager.ActionUsbDeviceAttached));

            DetachedReceiver = new UsbDeviceDetachedReceiver(this, LocContext);
            LocContext.RegisterReceiver(DetachedReceiver, new IntentFilter(UsbManager.ActionUsbDeviceDetached));
        }

        protected async Task<IList<IUsbSerialDriver>> GetDrivers()
        {
            //Adding drivers to the default probe table.
            //This code follows the direct example of the UsbSerialForAndroid library.
            ProbeTable ProbeInfo = UsbSerialProber.DefaultProbeTable;
            ProbeInfo.AddProduct(0x1b4f, 0x0008, typeof(CdcAcmSerialDriver)); // IOIO OTG
            ProbeInfo.AddProduct(0x09D8, 0x0420, typeof(CdcAcmSerialDriver)); // Elatec TWN4

            //Create a prober to probe for any recognized USBSerial device.
            UsbSerialProber USBProber = new UsbSerialProber(ProbeInfo);

            //Get all the drivers which match detected USBSerial devices.
            IList<IUsbSerialDriver> Drivers = await USBProber.FindAllDriversAsync(UsbManager);
            return Drivers;
        }

        //This follows the example set in the Xamarin USBSerial library closely.
        public async Task<bool> Connect()
        {
            //Get all the drivers which match detected USBSerial devices.
            IList<IUsbSerialDriver> Drivers = await GetDrivers();

            //Just get the first one.
            //Todo: Do this better. There's options to get specific devices, for example.
            IUsbSerialDriver Driver = Drivers.FirstOrDefault();

            if (Driver == null)
            {
                UpdateStatus(ConnectFailNoDriver);
                return false;
            }
            //Just get first port.
            UsbSerialPort port = Driver.Ports[0];
            if (port == null)
            {
                UpdateStatus(ConnectFailNoSerialDevice);
                return false;
            }

            //If given permission (only requests if permission is not given), initialize the Serial IO manager.
            if (await UsbManager.RequestPermissionAsync(Driver.Device, LocContext))
            {
                SerialIOManager = new SerialInputOutputManager(port)
                {
                    BaudRate = 9600,
                    DataBits = 8,
                    StopBits = StopBits.One,
                    Parity = Parity.None,
                };
                SerialIOManager.DataReceived += (Sender, Messenger) =>
                {
                    UpdateData?.Invoke(Encoding.UTF8.GetString(Messenger.Data));
                };
                SerialIOManager.ErrorReceived += (Sender, Messenger) =>
                {
                    UpdateStatus?.Invoke("Error: " + Messenger.ToString());
                };
                return true;
            }
            return false;
        }

        public async Task RunRecording()
        {
            try
            {
                SerialIOManager.Open(UsbManager);
            }
            catch (Java.IO.IOException e)
            {
                UpdateStatus?.Invoke("Error opening device: " + e.Message);
            }
            catch (Exception e)
            {
                UpdateStatus?.Invoke("Error:" + e.Message);
            }
        }

        public void StopRecording()
        {
            try
            {
                SerialIOManager.Close();
            }
            catch (Java.IO.IOException e)
            {
                UpdateStatus?.Invoke("Error closing device: " + e.Message);
            }
            catch (Exception e)
            {
                UpdateStatus?.Invoke("Error:" + e.Message);
            }
        }

        //To detect and ask for permission upon detecting a new USB connection.
        private class UsbDeviceAttachedReceiver
            : BroadcastReceiver
        {
            readonly string TAG = typeof(UsbDeviceAttachedReceiver).Name;
            readonly AndroidUsbSerialConnection Connection;

            Context LocContext;

            public UsbDeviceAttachedReceiver(AndroidUsbSerialConnection Connection, Context LocContext)
            {
                this.Connection = Connection;
                this.LocContext = LocContext;
            }

            public async override void OnReceive(Context context, Intent intent)
            {
                //Connection.UpdateStatus?.Invoke("Detected something was plugged in.");

                //Get all the drivers which match detected USBSerial devices.
                IList<IUsbSerialDriver> Drivers = await Connection.GetDrivers();

                //Just get the first one.
                //Todo: Do this better. There's options to get specific devices, for example.
                IUsbSerialDriver Driver = Drivers.FirstOrDefault();

                if (Driver == null)
                {
                    return;
                }
                //Just get 
                UsbSerialPort port = Driver.Ports[0];
                if (port == null)
                {
                    return;
                }

                // Get permission from the user to access the device (otherwise connection later will be null)
                await Connection.UsbManager.RequestPermissionAsync(Driver.Device, LocContext);
                return;
            }

        }
        //To detect USB detaching.
        private class UsbDeviceDetachedReceiver
            : BroadcastReceiver
        {
            readonly string TAG = typeof(UsbDeviceAttachedReceiver).Name;
            readonly AndroidUsbSerialConnection Connection;

            Context LocContext;

            public UsbDeviceDetachedReceiver(AndroidUsbSerialConnection Connection, Context LocContext)
            {
                this.Connection = Connection;
                this.LocContext = LocContext;
            }

            public async override void OnReceive(Context context, Intent intent)
            {
                //Connection.UpdateStatus?.Invoke("Detected something was unplugged.");

                //Get all the drivers which match detected USBSerial devices.
                IList<IUsbSerialDriver> Drivers = await Connection.GetDrivers();

            }
        }
    }
}