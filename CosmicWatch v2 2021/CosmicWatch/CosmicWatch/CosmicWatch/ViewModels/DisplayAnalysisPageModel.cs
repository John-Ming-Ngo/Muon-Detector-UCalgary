using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Forms;
using CosmicWatch.Models;

using CosmicWatch_Library;
using Xamarin.Essentials;
using System.Linq;

namespace CosmicWatch.ViewModels
{
    public class DisplayAnalysisPageModel
    {
        //[Displays/Data Exit Functions]
        public Action<List<String>> UpdateDataChoiceDisplay;
        public Action<List<ChartCreator.ChartTypes>> UpdateGraphChoiceDisplay;
        public Action<List<String>> UpdateXChoiceDisplay;
        public Action<List<String>> UpdateYChoiceDisplay;
        public Action<String> UpdateStatusDisplay;
        public Action<String> UpdateGraphDisplay;

        //Models interacted with.
        ReadFromFile recordings;

        //Data variables.
        
        //[Constructors]
        public DisplayAnalysisPageModel(Action<List<String>> UpdateDataChoiceDisplay, Action<List<ChartCreator.ChartTypes>> UpdateGraphChoiceDisplay, Action<List<String>> UpdateXChoiceDisplay, Action<List<String>> UpdateYChoiceDisplay, Action<String> UpdateStatusDisplay, Action<String> UpdateGraphDisplay)
        {
            //Displays
            this.UpdateDataChoiceDisplay += UpdateDataChoiceDisplay;
            this.UpdateGraphChoiceDisplay += UpdateGraphChoiceDisplay;
            this.UpdateXChoiceDisplay += UpdateXChoiceDisplay;
            this.UpdateYChoiceDisplay += UpdateYChoiceDisplay;
            this.UpdateStatusDisplay += UpdateStatusDisplay;
            this.UpdateGraphDisplay += UpdateGraphDisplay;

            //Initialize models
            recordings = new ReadFromFile();

            //Initialize Data
            UpdateDataChoiceDisplay(new List<String>(recordings.GetFiles()));
            UpdateGraphChoiceDisplay(new List<ChartCreator.ChartTypes> { ChartCreator.ChartTypes.Bar, ChartCreator.ChartTypes.Line, ChartCreator.ChartTypes.Scatter });
        }
        /**/
        Dictionary<String, List<String>> DataChoices;

        ChartCreator.ChartTypes chartType;
        List<String> XList;
        List<String> YList;
        String Title = "";
        String XLabel;
        String YLabel;
        bool BeginAtZero = true;

        const int MAX_READ_LINES = 10000;

        private bool DataInitialized()
        {
            return (chartType != null && XList != null && YList != null && Title != null && XLabel != null && YLabel != null && BeginAtZero != null);
        }

        public void SelectDataChoice(String filename)
        {
            List<String> NextData()
            {
                if (recordings == null || recordings.EndOfFile) return new List<String>();
                String labels = recordings.ReadLine();
                return new List<String>(labels.Split(' '));
                //return new List<String>(labels.Split(','));
            }

            recordings.Open(filename);
            List<String> labelsList = NextData();

            UpdateXChoiceDisplay(labelsList);
            UpdateYChoiceDisplay(labelsList);

            DataChoices = new Dictionary<string, List<string>>();
            foreach (String label in labelsList)
            {
                DataChoices.Add(label, new List<String>());
            }

            //Read the data, to the maximum amount possible.
            
            for (int currRow = 0; currRow < MAX_READ_LINES && !recordings.EndOfFile; currRow++)
            {
                List<String> resultsList = NextData();
                for (int labelNumber = 0; labelNumber < labelsList.Count; labelNumber++)
                {
                    //_ = labelsList[labelNumber];
                    //_ = resultsList[labelNumber];
                    DataChoices[labelsList[labelNumber]].Add(resultsList[labelNumber]);
                }
            }
        }

        public void SelectChartType(ChartCreator.ChartTypes selectedType)
        {
            chartType = selectedType;
            UpdateGraph();
        }

        public void SelectXList(String selection)
        {
            XLabel = selection;
            XList = DataChoices[XLabel];
            
            //UpdateStatusDisplay(String.Join(",", XList));
            UpdateGraph();
        }

        public void SelectYList(String selection)
        {
            YLabel = selection;
            YList = DataChoices[YLabel];

            //UpdateStatusDisplay(String.Join(",", ToDoublesList(YList).Select(x => $"{x}")));
            //UpdateStatusDisplay(String.Join(",", YList));
            UpdateGraph();
        }

        private List<Double> ToDoublesList(List<String> inputString)
        {
            if (inputString != null) return inputString.Select(s => double.TryParse(s, out double n) ? n : 0)//n : (double?)null)
                //.Where(n => n.HasValue)
                //.Select(n => n.Value)
                .ToList();
            return new List<double>();
        }

        public void UpdateGraph()
        {
            //UpdateStatusDisplay(String.Join(",", ToDoublesList(YList).Select(x => $"{x}")) + String.Join(",", XList));
            if (!DataInitialized()) return;

            switch (chartType)
            {
                case ChartCreator.ChartTypes.Line:
                    //UpdateGraphDisplay(ChartCreator.LineChartString(new List<String> { "A", "B", "C", "D", "E" }, new List<double> { 10, 20, 30, 40, 50 }, "Title", "XLabel", "YLabel", true));
                    UpdateGraphDisplay(ChartCreator.LineChartString(XList, ToDoublesList(YList), Title, XLabel, YLabel, BeginAtZero));
                    break;
                case ChartCreator.ChartTypes.Bar:
                    //UpdateGraphDisplay(ChartCreator.BarChartString(new List<String> { "A", "B", "C", "D", "E" }, new List<double> { 10, 20, 30, 40, 50 }, "Title", "XLabel", "YLabel", true));
                    UpdateGraphDisplay(ChartCreator.BarChartString(XList, ToDoublesList(YList), Title, XLabel, YLabel, BeginAtZero));
                    break;
                case ChartCreator.ChartTypes.Scatter:
                    //Wait, how to convert xList? Drat.
                    //Instead of doing it here, maybe not? Maybe we simply take strings, and have something else create the chartString.
                    //UpdateGraphDisplay(ChartCreator.ScatterChartString(new List<double> { 10, 20, 30, 40, 50 }, new List<double> { 10, 20, 30, 40, 50 }, "Title", "XLabel", "YLabel", true));
                    UpdateGraphDisplay(ChartCreator.ScatterChartString(ToDoublesList(XList), ToDoublesList(YList), Title, XLabel, YLabel, BeginAtZero));
                    break;
                default:
                    break;
            }
        }

    }
}
