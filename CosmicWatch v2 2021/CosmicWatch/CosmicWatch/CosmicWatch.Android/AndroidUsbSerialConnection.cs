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

//Current problem: Get permission without crash-refreshing the entire app!

//Not complete; want to detect USB attached, not availiable yet.
//https://github.com/anotherlab/UsbSerialForAndroid
//https://docs.microsoft.com/en-us/xamarin/android/platform/binding-java-library/
//https://forums.xamarin.com/discussion/43810/usb-communication-android-phone-with-special-hardware-with-phone-modul-and-usb-bluetooth
//https://docs.microsoft.com/en-us/answers/questions/213419/xamarin-usb-host-android-request-permission-to-con.html

[assembly: UsesFeature("android.hardware.usb.host")]
[assembly: Dependency(typeof(AndroidUsbSerialConnection))]

namespace CosmicWatch.Droid
{
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] { UsbManager.ActionUsbDeviceAttached })]
    [MetaData(UsbManager.ActionUsbDeviceAttached, Resource = "@xml/device_filter")]
    public class AndroidUsbSerialConnection : IUSBSerialConnection
    {
        //Communication functions
        //Allows the connection to communicate back its data and status in the method preferred by the implementer.
        Action<String> UpdateData;
        Action<String> UpdateStatus;

        //Serial Connection and Reader attributes.
        //This is what this class was built to wrap.
        UsbManager UsbManager;
        //-UsbSerialPortAdapter adapter;
        //-BroadcastReceiver detachedReceiver;
        //-UsbSerialPort selectedPort;
        SerialInputOutputManager serialIoManager;

        //Global variables.
        Context LocContext;

        //Settings variables.
        uint MaxReadLength;

        public void Initialize(Action<String> UpdateDataOutput, Action<String> UpdateStatusMessage, uint MaxReadLength)
        {
            this.UpdateData = UpdateDataOutput;
            this.UpdateStatus = UpdateStatusMessage;
            this.MaxReadLength = MaxReadLength;

            LocContext = Android.App.Application.Context;
            UsbManager = (UsbManager)LocContext.GetSystemService(Context.UsbService);

            //Detect and ask for permission upon detecting a new USB connection.
            UsbDeviceDetachedReceiver detachedReceiver = new UsbDeviceDetachedReceiver(this);
            LocContext.RegisterReceiver(detachedReceiver, new IntentFilter(UsbManager.ActionUsbDeviceDetached));
        }

        //This follows the example set in the Xamarin USBSerial library closely.
        public async Task<bool> Connect() 
        {

            //Adding drivers to the default probe table.
            //Following the example set in the Hoho.Android.UsbSerial library.
            ProbeTable probeInfo = UsbSerialProber.DefaultProbeTable;
            probeInfo.AddProduct(0x1b4f, 0x0008, typeof(CdcAcmSerialDriver)); // IOIO OTG
            probeInfo.AddProduct(0x09D8, 0x0420, typeof(CdcAcmSerialDriver)); // Elatec TWN4

            //Create a prober to probe for any recognized USBSerial device.
            UsbSerialProber prober = new UsbSerialProber(probeInfo);

            //Get all the drivers which match detected USBSerial devices.
            IList<IUsbSerialDriver> drivers = await prober.FindAllDriversAsync(UsbManager);

            //Just get the first one.
            //Todo: Do this better. There's options to get specific devices, for example.
            IUsbSerialDriver driver = drivers.FirstOrDefault();
            
            if (driver == null) { 
                UpdateStatus("No driver!");
                return false;
            }
            //Just get 
            UsbSerialPort port = driver.Ports[0];
            if (port == null)
            {
                UpdateStatus("No serial device.");
                return false;
            }

            // Get permission from the user to access the device (otherwise connection later will be null)
            if (!UsbManager.HasPermission(driver.Device))
            {
                UsbManager.RequestPermission(driver.Device, null);
            }
            
            serialIoManager = new SerialInputOutputManager(port)
            {
                BaudRate = 9600,
                DataBits = 8,
                StopBits = StopBits.One,
                Parity = Parity.None,
            };
            serialIoManager.DataReceived += (sender, e) => {
                UpdateData?.Invoke(Encoding.UTF8.GetString(e.Data));
                //UpdateStatus?.Invoke(Encoding.UTF8.GetString(e.Data));
            };
            serialIoManager.ErrorReceived += (sender, e) => {
                //UpdateStatus?.Invoke("Error received:" + e.ToString());
            };
            return true;
        }

        public void RunRecording()
        {
            try
            {
                serialIoManager.Open(UsbManager);
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
                serialIoManager.Close();
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
        class UsbDeviceDetachedReceiver
            : BroadcastReceiver
        {
            readonly string TAG = typeof(UsbDeviceDetachedReceiver).Name;
            readonly AndroidUsbSerialConnection connection;
            public UsbDeviceDetachedReceiver(AndroidUsbSerialConnection connection)
            {
                this.connection = connection;
            }
                        
            public override void OnReceive(Context context, Intent intent)
            {
                UsbDevice device = intent.GetParcelableExtra(UsbManager.ExtraDevice) as UsbDevice;

                if (connection.UsbManager.HasPermission(device))
                {
                    connection.UsbManager.RequestPermission(device, null);
                }


                Log.Info(TAG, "USB device detached: " + device.DeviceName);

                //await activity.PopulateListAsync();
                String value = intent.GetStringExtra("key");
            }
        }
    }
}