using System;
using System.Timers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using CosmicWatch.Views;
using CosmicWatch.ViewModels;

using CosmicWatch_Library;
using Xamarin.Essentials;

namespace CosmicWatch.Views
{
    public partial class DisplayAnalysisPage : ContentPage
    {
        //[Viewmodel]
        DisplayAnalysisPageModel pageModel;

        //[Constructor]
        public DisplayAnalysisPage()
        {
            InitializeComponent();
            //Initialize Viewmodel
            pageModel = new DisplayAnalysisPageModel(UpdateDataChoiceDisplay, UpdateGraphChoiceDisplay, UpdateXChoiceDisplay, UpdateYChoiceDisplay, UpdateStatusMessage, UpdateGraphDisplay);

            //
            //UpdateGraphDisplay(ChartCreator.ScatterChartString(new List<double> { 10, 20, 30, 40, 50 }, new List<double> { 10, 20, 30, 40, 50 }, "Title", "XLabel", "YLabel", true));
            //UpdateGraphDisplay(ChartCreator.LineChartString(new List<String> { "A", "B", "C", "D", "E" }, new List<double> { 10, 20, 30, 40, 50 }, "Title", "XLabel", "YLabel", true));
            UpdateGraphDisplay(ChartCreator.BarChartString(new List<String> { "A", "B", "C", "D", "E" }, new List<double> { 10, 20, 30, 40, 50 }, "Title", "XLabel", "YLabel", true));

            //SaveToFile exportFile = new SaveToFile("BarChartString.html");
            //exportFile.WriteLine(ChartCreator.BarChartString(new List<String> { "A", "B", "C", "D", "E" }, new List<double> { 10, 20, 30, 40, 50 }, "Title", "XLabel", "YLabel", true));
            //exportFile.Close();
        }
        private void OnDatasetSelect(object sender, EventArgs e)
        {
            Picker selector = (Picker)sender;
            String selectedFile = (String)selector.SelectedItem;
            pageModel.SelectDataChoice(selectedFile);
        }
        private void OnGraphTypeSelect(object sender, EventArgs e)
        {
            Picker selector = (Picker)sender;
            ChartCreator.ChartTypes selectedGraphType = (ChartCreator.ChartTypes)selector.SelectedItem;
            pageModel.SelectChartType(selectedGraphType);
        }
        private void OnXSelect(object sender, EventArgs e)
        {
            Picker selector = (Picker)sender;
            String selectedX = (String)selector.SelectedItem;
            pageModel.SelectXList(selectedX);

        }
        private void OnYSelect(object sender, EventArgs e)
        {
            Picker selector = (Picker)sender;
            String selectedY = (String)selector.SelectedItem;
            pageModel.SelectYList(selectedY);
        }

        private void UpdateDataChoiceDisplay(List<String> DataChoices)
        {
            Device.BeginInvokeOnMainThread(() => Datasets.ItemsSource = DataChoices);

            var someVar = Datasets.ItemsSource;
        }
        private void UpdateGraphChoiceDisplay(List<ChartCreator.ChartTypes> DataChoices)
        {
            Device.BeginInvokeOnMainThread(() => Display_Type.ItemsSource = DataChoices);
        }
        private void UpdateXChoiceDisplay(List<String> DataChoices)
        {
            Device.BeginInvokeOnMainThread(() => X_Axis.ItemsSource = DataChoices);
        }
        private void UpdateYChoiceDisplay(List<String> DataChoices)
        {
            Device.BeginInvokeOnMainThread(() => Y_Axis.ItemsSource = DataChoices);
        }
        private void UpdateStatusMessage(String message)
        {
            Device.BeginInvokeOnMainThread(() => statusDisplay.Text = message);
        }
        private void UpdateGraphDisplay(String ChartHTML)
        {
            HtmlSource.Html = ChartHTML;
            //Device.BeginInvokeOnMainThread(() => HtmlSource.Html = ChartHTML);
        }
    }
}