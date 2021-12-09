using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using CosmicWatch_Library;

namespace CosmicWatch.Views.UseGuideSubPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BuildDetectorPage : ContentPage
    {
        //[Display Variables]
        private String mainText;
        private String MainText
        {
            get
            {
                return mainText;
            }
            set
            {
                mainText = value;
                MainTextDisplay.Text = mainText;
            }
        }
        private String mainTitle;
        private String MainTitle
        {
            get
            {
                return mainTitle;
            }
            set
            {
                MainTextTitle.Text = value;
            }
        }
        public BuildDetectorPage()
        {
            InitializeComponent();
            //[Initialize Page Display Variables]
            MainTitle = AssortedUtil.GetEmbeddedText(typeof(BuildDetectorPage), "CosmicWatch.Resources.Text.UseGuideSubPagesText.BuildDetectorPageText.MainTitle.txt");
            MainText = AssortedUtil.GetEmbeddedText(typeof(BuildDetectorPage), "CosmicWatch.Resources.Text.UseGuideSubPagesText.BuildDetectorPageText.MainText.txt");
        }
    }
}