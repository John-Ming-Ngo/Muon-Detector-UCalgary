using CosmicWatch_Library;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;


//https://docs.microsoft.com/en-us/dotnet/api/system.idisposable?view=net-5.0
//https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/destructors

namespace CosmicWatch.Models
{
    /*
     Purpose: 
        Recorder and Processor for the detector's data.
        Processes and saves string data from the detector into a comprehensible format.
        
    Organization of this page:
        1. Data and References
        2. Functionality.

    Todo: Implement idisposable rather than what I did here.
     */
    public class DetectionRecord
    {
        //[Core Record Data]
        private int eventCount;
        public int EventCount { 
            get { return eventCount; }
            private set { eventCount = value; }
        }

        public Stopwatch stopwatch;
        public double EventsPerMinute
        {
            get { return (stopwatch != null) ? eventCount / (stopwatch.ElapsedMilliseconds / 1000.0 / 60) : 0; }
        }

        //Saving the data to an output file.
        SaveToFile RawDataRecord;

        //Buffers and Processing Information
        String DataBuffer;
        String[] SplitSymbols;
        String[] RejectSymbols;

        //[Constructor]
        public DetectionRecord(String filename)
        {
            //Initialize 
            RawDataRecord = new SaveToFile(filename);
            
            RawDataRecord.WriteLine(String.Join(" ", new String[]{ "Counts", "Ardn_time[ms]", "Amplitude[mV]", "SiPM[mV]", "Deadtime[ms]"}));

            EventCount = 0;

            DataBuffer = "";
            SplitSymbols = new String[]{"\n"};

            //It turns out that I don't need to get rid of the other symbols; on the contrary I need to simply ignore them. That is unbelievably weird.
            //RejectSymbols = new String[] { "\r\n", "\r" };

            stopwatch = new Stopwatch();
            stopwatch.Start();
        }
        //Receive and process new data.
        public void ReceiveData(String Input)
        {
            //Sanitize the Input.
            String SanitizedInput = Input;
            //It turns out that I don't need to get rid of the other symbols; on the contrary I need to simply ignore them. That is unbelievably weird.
            //String.Join("", Input.Split(RejectSymbols, StringSplitOptions.None), String.Empty);
            //Add the input to the buffer, and then parse the input.
            DataBuffer += SanitizedInput;
            ParseBufferDetections();
        }

        //Parse the buffer, incrementing the counter by how many distinct events are found.
        private void ParseBufferDetections()
        {
            String[] EventsDetected = DataBuffer.Split(SplitSymbols, StringSplitOptions.None);
            int NumLastLine = EventsDetected.Length - 1;
            //Want to not take all the data, but until 1 to the last... do I have to use a normal for loop? Goddamn.
            for (int DataLineIndex = 0; DataLineIndex < NumLastLine; DataLineIndex++)
            {
                RawDataRecord.WriteLine(EventsDetected[DataLineIndex]);
            }
            DataBuffer = EventsDetected[NumLastLine];
            EventCount += Math.Max(EventsDetected.Length - 1, 0);
        }
        //Close and output classes this class depends upon.
        public void Close()
        {
            stopwatch?.Stop();
            RawDataRecord?.Close();
        }
        //[Destructor in case Close was not called]
        ~DetectionRecord()
        {
            Close();
        }


    }
}
