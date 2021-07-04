using CosmicWatch_Library;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;


//https://docs.microsoft.com/en-us/dotnet/api/system.idisposable?view=net-5.0
//https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/destructors

namespace CosmicWatch.Models
{
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

        SaveToFile RawDataRecord;

        //Buffers and Processing Information
        String DataBuffer;
        String[] SplitSymbols;

        //[Constructor]
        public DetectionRecord(String filename)
        {
            //Initialize 
            RawDataRecord = new SaveToFile(filename);
            EventCount = 0;

            DataBuffer = "";
            SplitSymbols = new String[]{"\r\n", "\r", "\n"};

            stopwatch = new Stopwatch();
            stopwatch.Start();
        }
        //Receive and process new data.
        public void ReceiveData(String input)
        {
            DataBuffer += input;
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
        //
        public void Close()
        {
            stopwatch.Stop();
            RawDataRecord.Close();
        }
        //[Destructor in case Close was not called]
        ~DetectionRecord()
        {
            Close();
        }


    }
}
