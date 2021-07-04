using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

/*
 Generates an embedded HTML/Javascript string which can be used in a Xamarin Forms webview to see a graph.
 */
namespace CosmicWatch_Library
{
	public static class ChartCreator
	{
		/*
		 Constants - Javascript source, and formatting.
		 */

		private static String ChartJSSource = AssortedUtil.GetEmbeddedText(typeof(ChartCreator), "CosmicWatch_Library.ChartCreatorResources.Chartmin.js");

		private static String ChartJSUtils = AssortedUtil.GetEmbeddedText(typeof(ChartCreator), "CosmicWatch_Library.ChartCreatorResources.ChartUtil.js");

		//private static String ChartJSSource = "https://www.chartjs.org/dist/2.9.4/Chart.min.js";
		//private static String ChartJSUtils = "https://www.chartjs.org/samples/latest/utils.js"; //This is broken, need to replace it.
		private static String ChartSeparator = ",";

		//CHARTING COMMON SETTINGS
		public enum ChartTypes
		{
			Line,
			Bar,
			Scatter
		}

		/*
		 Common Canvas Settings for the chart.
		 */
		public static String CanvasSettings()
        {
			return $@"
			canvas {{
				margin: auto;
				}}";
        }

		public static String Scripts()
        {
			return $@"
				<script>{ChartJSSource}</script>
				<script>{ChartJSUtils}</script>
			";
        }
		//CHARTING FUNCTIONS

		/*
		 Bar Chart
		 */
		public static String BarChartString(List<String> xLabels, List<double> yData, String title, String xLabel, String yLabel, bool beginAtZeroBool)
		{
			//Formatting input data into a form which works for Javascript.
			String beginAtZero = beginAtZeroBool.ToString().ToLower();

			String barLabels = StringList(xLabels);
			String barData = StringList(yData);
			String barColors = StringList(GenerateRandomColors(xLabels.Count));

			//
			return $@"
                <html>
				<head>
				<style>
					{CanvasSettings()}
				</style>
				</head>
                <body>
				{Scripts()}
                <div class='myChartContainer' style='position: relative; height: 90vh; width: 90vw'>
                <canvas id = 'myChart'></canvas>
                </div>
                <script>
                var chartScreen = document.getElementById('myChart');
                var myChart = new Chart(chartScreen, {{
                    type: 'bar',
                    data: {{
                        labels: [{barLabels}],
                        datasets: [{{
                            label: '# of Votes',
                        data: [{barData}],
                        backgroundColor: [{barColors}],
                        borderColor: [{barColors}],
                        borderWidth: 1
                        }}]
                    }},
                    options: {{
							responsive: true, 
							maintainAspectRatio: false,
							title: {{
						        display: true,
								text: '{title}'
								}},
							scales: {{
								xAxes: [{{
									display: true,
									scaleLabel: {{
										display: true,
										labelString: '{xLabel}'
										}}
									}}],
								yAxes: [{{
									ticks: {{
									beginAtZero: {beginAtZero}
									}},
									display: true,
									scaleLabel: {{
										display: true,
										labelString: '{yLabel}'
										}}
									}}]
								}}
							}}
						}});
                </script>
                </body>
                </html>";
		}
		/*
		 Scatter Chart
		 */
		public static String ScatterChartString(List<double> xData, List<double> yData, String title, String xLabel, String yLabel, bool beginAtZeroBool)
		{
			//Formatting input data into a form which works for Javascript.
			String beginAtZero = beginAtZeroBool.ToString().ToLower();

			String dataLabel = "TestLabel"; //Todo: Need to make this a parameter eventually.
			String scatterData = StringList(xData, yData);

			//
			return $@"
                <html>
				<head>
				<style>
					{CanvasSettings()}
				</style>
				</head>
                <body>
				{Scripts()}
                <div class='myChartContainer' style='position: relative; height: 90vh; width: 90vw'>
                <canvas id = 'myChart'></canvas>
                </div>
                <script>
		            var color = Chart.helpers.color;
		            var scatterChartData = {{
			            datasets: [{{
							label: '{dataLabel}',
				            borderColor: window.chartColors.red,
				            backgroundColor: color(window.chartColors.red).alpha(0.2).rgbString(),
				            data: [{scatterData}]
			            }}]
		            }};

		            var myChart = document.getElementById('myChart').getContext('2d');
		            window.myScatter = Chart.Scatter(myChart, {{
			            data: scatterChartData,
			            options: {{
							responsive: true, 
							maintainAspectRatio: false,
				            title: {{
					            display: true,
								text: '{title}'
								}},
						scales: {{
							xAxes: [{{
								display: true,
								scaleLabel: {{
									display: true,
									labelString: '{xLabel}'
									}}
								}}],
							yAxes: [{{
								ticks: {{
								beginAtZero: {beginAtZero}
								}},
								display: true,
								scaleLabel: {{
									display: true,
									labelString: '{yLabel}'
									}}
								}}]
							}}
			            }}
		            }});
	            </script>
                </body>
                </html>";
		}
		/*
		 Line Chart
		 */
		public static String LineChartString(List<String> xLabels, List<double> yData, String title, String xLabel, String yLabel, bool beginAtZeroBool)
		{
			//Formatting input data into a form which works for Javascript.
			String beginAtZero = beginAtZeroBool.ToString().ToLower();

			String lineLabels = StringList(xLabels);
			String lineData = StringList(yData);
			String lineColor;

			//
			return $@"
                <html>
				<head>
				<style>
					{CanvasSettings()}
				</style>
				</head>
                <body>
				{Scripts()}
                <div class='myChartContainer' style='position: relative; height: 90vh; width: 90vw'>
                <canvas id = 'myChart'></canvas>
                </div>
                <script>
		            var config = {{
					type: 'line',
					data: {{
						labels: [{lineLabels}],
						datasets: [{{
						label: 'Unfilled',
						fill: false,
						backgroundColor: window.chartColors.blue,
						borderColor: window.chartColors.blue,
						data:[{lineData}],
						}}]
					}},
					options: {{
						responsive: true, 
						maintainAspectRatio: false,
						title: {{
						            display: true,
									text: '{title}'
						}},
						tooltips: {{
							mode: 'index',
							intersect: false,
						}},
						hover: {{
							mode: 'nearest',
							intersect: true
						}},
						scales: {{
							xAxes: [{{
								display: true,
								scaleLabel: {{
									display: true,
									labelString: '{xLabel}'
									}}
							}}],
							yAxes: [{{
								ticks: {{
								beginAtZero: {beginAtZero}
								}},
								display: true,
								scaleLabel: {{
									display: true,
									labelString: '{yLabel}'
									}}
								}}]
							}}
						}}
					}};

					var chart = document.getElementById('myChart').getContext('2d');
					window.myLine = new Chart(chart, config);

	            </script>
                </body>
                </html>";
        }
		
		//UTILITY FUNCTIONS
		
		/*
		 Generating Random Colours
		 */
		public static List<String> GenerateRandomColors(int numberOfColours)
        {
			Random random = new Random();

			List<String> randomColors = new List<String>();

			for (int i = 0; i < numberOfColours; i++)
            {
				randomColors.Add(String.Format("#{0:X6}", random.Next(0x1000000)));
            }
			return randomColors;
		}
		/*
		 StringList - Turns the contents of the list into a string readable by a javascipt program. Used to aid the prior charting functions.
		 Handles doubles list.

		 */
		public static String StringList(List<Double> inputList)
        {
            return String.Join(ChartSeparator, inputList);
        }
		/*
		 StringList - Turns the contents of the list into a string readable by a javascipt program. Used to aid the prior charting functions.
		 Handles String list.
		 */
		public static String StringList(List<String> inputList)
        {
			List<String> formattedInput = new List<String>();
			foreach (String input in inputList) {
				formattedInput.Add("'" + input + "'");
            }
			return String.Join(ChartSeparator, formattedInput);
		}
		/*
		 StringList - Turns the contents of the list into a string readable by a javascipt program. Used to aid the prior charting functions.
		 Handles Scatterplot data with a list of x inputs, and a list of y inputs.
		 */
		public static String StringList(List<Double> xList, List<Double> yList)
        {
			List<String> formattedInput = new List<String>();
			for (int i = 0; i < (xList.Count) && i < yList.Count; i++)
            {
				formattedInput.Add($"{{x: {xList[i]}, y: {yList[i]},}}");
            }

			return String.Join(ChartSeparator, formattedInput);
		}
    }
	
}
