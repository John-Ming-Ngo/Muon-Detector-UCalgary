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
    /*
    Purpose: 
        Main view model of the Display Analysis page.
        Handles the data manipulation for the Display Analysis page.
   
    Organization of this page:
        1. Data and References
        2. By location on the screen (Top first, bottom last)
    
    On Interactions with the Display Analysis Page:
        Takes arguments from the Analysis page:
            Through the contructor
            Via public functions
        Returns data to the main page:
            Via display update functions.
        
        Intended Pattern of Data Transfer:
            Page to Page Model:
                What to do.
            Page Model to Page:
                What to display.
        
        Anything else is bad and needs to be refactored.

        Models Interacted With:
            ReadFromFile:
                Purpose: Read previously saved data files, and load them into memory for this page model to manipulate.
                From Model: Availiable files to read, data contents of these files, as functions.
                To Model: Which file to read, as string, input as function.
    */
    public class DisplayAnalysisPageModel
    {
        //[Displays/Data Exit Functions]
        public Action<List<String>> UpdateDataChoiceDisplay;
        public Action<List<ChartCreator.ChartTypes>> UpdateGraphChoiceDisplay;
        public Action<List<String>> UpdateXChoiceDisplay;
        public Action<List<String>> UpdateYChoiceDisplay;
        public Action<String> UpdateStatusDisplay;
        public Action<String> UpdateGraphDisplay;

        //[Models]
        private ReadFromFile Recordings;

        //[Data variables]
        
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
            Recordings = new ReadFromFile();

            //Initialize Data
            UpdateDataChoiceDisplay(new List<String>(Recordings.GetFiles()));
            UpdateGraphChoiceDisplay(new List<ChartCreator.ChartTypes> { ChartCreator.ChartTypes.Bar, ChartCreator.ChartTypes.Line, ChartCreator.ChartTypes.Scatter });
        }

        /*Webview Graph Display Functions and Data Variables!*/

        //[CSV Data: Label and Data under the Label]
        private Dictionary<String, List<String>> DataChoices;

        //[Graph Data: Information the graph needs to function]
        private ChartCreator.ChartTypes chartType;
        private List<String> XList;
        private List<String> YList;
        private String Title = "";
        private String XLabel;
        private String YLabel;
        private bool BeginAtZero = true;

        //[Maximum Read Lines: To Prevent Reading too Much Data and Crashing]
        //Todo: Should be dynamically done?
        const int MAX_READ_LINES = 10000;
        char DataSplitSymbol = ',';
        //[On Data Selected Functions]
        //These functions are called by the page's picker functions, and select the data to be displayed in the graph.
        public void SelectDataChoice(String filename)
        {
            //Function to get the next bit of data in the selected file.
            List<String> NextData()
            {
                if (Recordings == null || Recordings.EndOfFile) return new List<String>();
                String labels = Recordings.ReadLine();
                return new List<String>(labels.Split(DataSplitSymbol));
                //return new List<String>(labels.Split(','));
            }

            //Data Validation
            if (filename == null)
            {
                return;
            }

            //Open the selected file.
            Recordings.Open(filename);

            //Get the first row of data. In a normal CSV, that contains the labels. We then update the choice of X and Y axis with these labels.
            List<String> labelsList = NextData();
            UpdateXChoiceDisplay(labelsList);
            UpdateYChoiceDisplay(labelsList);

            //We initialize the Data Choices with these data labels and a list ready to contain the data under each label.
            DataChoices = new Dictionary<string, List<string>>();
            foreach (String label in labelsList)
            {
                DataChoices.Add(label, new List<String>());
            }

            //Read the data, to the maximum amount possible.
            //Then put this data under the list under the appropriate label.
            for (int currRow = 0; currRow < MAX_READ_LINES && !Recordings.EndOfFile; currRow++)
            {
                List<String> resultsList = NextData();
                for (int labelNumber = 0; labelNumber < labelsList.Count && labelNumber < resultsList.Count; labelNumber++)
                {
                    DataChoices[labelsList[labelNumber]].Add(resultsList[labelNumber]);
                }
            }
            Title = filename;
        }

        public void SelectChartType(ChartCreator.ChartTypes selectedType)
        {
            //Set the chart type
            chartType = selectedType;
            //Update the graph
            UpdateGraph();
        }

        public void SelectXList(String selection)
        {
            if (selection == null)
            {
                XLabel = null;
                XList = null;
                return;
            }
            //Set the X Axis label and the data of the X Axis
            XLabel = selection;
            XList = DataChoices[XLabel];
            //Update the graph
            UpdateGraph();
        }

        public void SelectYList(String selection)
        {
            if (selection == null)
            {
                YLabel = null;
                YList = null;
                return;
            }
            //Set the Y Axis label and the data of the Y Axis
            YLabel = selection;
            YList = DataChoices[YLabel];
            //Update the graph
            UpdateGraph();
        }

        public void DeleteData()
        {
            Recordings.DeleteFile();
            UpdateDataChoiceDisplay(new List<String>(Recordings.GetFiles()));
            UpdateStatusDisplay("Dataset deleted from system memory. The data will remain loaded in active memory until you select a new dataset.");
        }

        public void UpdateGraph()
        {
            //Function to check if all the data has been initialized and can be fed into the UpdateGraphDisplay function.
            bool DataInitialized()
            {
                return (XList != null && 
                        YList != null && 
                        Title != null && 
                        XLabel != null && 
                        YLabel != null);
            }
            //Function to convert the data to a list of doubles, so that the chart creator graph creation functions can accept the data.
            List<Double> ToDoublesList(List<String> inputString)
            {
                if (inputString != null) 
                    return inputString.Select(x => double.TryParse(x, out double value) ? value : 0).ToList();
                else return new List<double>();
            }

            //If not all the data is availiable, don't do anything.
            if (!DataInitialized()) return;

            //Else, identify what type of chart we want to display, and then display it.
            switch (chartType)
            {
                case ChartCreator.ChartTypes.Line:
                    UpdateGraphDisplay(
                        ChartCreator.LineChartString(
                            XList, 
                            ToDoublesList(YList), 
                            Title, 
                            XLabel, 
                            YLabel, 
                            BeginAtZero
                            )
                        );
                    break;
                case ChartCreator.ChartTypes.Bar:
                    UpdateGraphDisplay(
                        ChartCreator.BarChartString(
                            XList, 
                            ToDoublesList(YList), 
                            Title, 
                            XLabel, 
                            YLabel, 
                            BeginAtZero
                            )
                        );
                    break;
                case ChartCreator.ChartTypes.Scatter:
                    UpdateGraphDisplay(
                        ChartCreator.ScatterChartString(
                            ToDoublesList(XList), 
                            ToDoublesList(YList), 
                            Title, 
                            XLabel, 
                            YLabel, 
                            BeginAtZero
                            )
                        );
                    break;
                default:
                    break;
            }
        }

    }
}
