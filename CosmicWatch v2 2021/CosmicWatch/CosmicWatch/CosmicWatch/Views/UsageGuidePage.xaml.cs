using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using CosmicWatch_Library;

namespace CosmicWatch.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UsageGuidePage : ContentPage
    {
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
        public UsageGuidePage()
        {
            InitializeComponent();

            MainTitle = AssortedUtil.GetEmbeddedText(typeof(UsageGuidePage), "CosmicWatch.Resources.Text.UseGuidePageText.MainTitle.txt");
            MainText = AssortedUtil.GetEmbeddedText(typeof(UsageGuidePage), "CosmicWatch.Resources.Text.UseGuidePageText.MainText.txt");
        }

        private void OnAboutTheScience(object sender, EventArgs e)
        {
            Navigation.PushAsync(new UseGuideSubPages.AboutTheSciencePage());
        }
        private void OnBuildDetector(object sender, EventArgs e)
        {
            Navigation.PushAsync(new UseGuideSubPages.BuildDetectorPage());
        }
        private void OnHowToUse(object sender, EventArgs e)
        {
            Navigation.PushAsync(new UseGuideSubPages.HowToUsePage());
        }
        private void OnVideoDemonstration(object sender, EventArgs e)
        {
            Navigation.PushAsync(new UseGuideSubPages.VideoDemonstrationPage());
        }
    }
}