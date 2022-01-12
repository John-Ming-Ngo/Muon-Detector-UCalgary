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
        //[Constructor]
        public UsageGuidePage()
        {
            InitializeComponent();
            //[Initialize Page Display Variables]
            InitializeToolbarIcons();
            MainTextTitle.Text = AssortedUtil.GetEmbeddedText(typeof(UsageGuidePage), "CosmicWatch.Resources.Text.UseGuidePageText.MainTitle.txt");
            //[Text and Images: Row 1]
            MainTextDisplay.Text = AssortedUtil.GetEmbeddedText(typeof(UsageGuidePage), "CosmicWatch.Resources.Text.UseGuidePageText.Text0.txt");
            MainImage.Source = AssortedUtil.GetEmbeddedImage(typeof(UsageGuidePage), "CosmicWatch.Resources.Images.UseGuidePageImages.Image0.png");
            //[Text and Images: Row 2]
            MainTextDisplay1.Text = AssortedUtil.GetEmbeddedText(typeof(UsageGuidePage), "CosmicWatch.Resources.Text.UseGuidePageText.Text1.txt");
            MainImage1.Source = AssortedUtil.GetEmbeddedImage(typeof(UsageGuidePage), "CosmicWatch.Resources.Images.UseGuidePageImages.Image1.png");
            //[Text and Images: Row 3]
            MainTextDisplay2.Text = AssortedUtil.GetEmbeddedText(typeof(UsageGuidePage), "CosmicWatch.Resources.Text.UseGuidePageText.Text2.txt");
            MainImage2.Source = AssortedUtil.GetEmbeddedImage(typeof(UsageGuidePage), "CosmicWatch.Resources.Images.UseGuidePageImages.Image2.png");
            //[Text and Images: Row 4]
            MainTextDisplay3.Text = AssortedUtil.GetEmbeddedText(typeof(UsageGuidePage), "CosmicWatch.Resources.Text.UseGuidePageText.Text3.txt");
            MainImage3.Source = AssortedUtil.GetEmbeddedImage(typeof(UsageGuidePage), "CosmicWatch.Resources.Images.UseGuidePageImages.Image3.png");
            //[Text and Images: Row 5]
            MainTextDisplay4.Text = AssortedUtil.GetEmbeddedText(typeof(UsageGuidePage), "CosmicWatch.Resources.Text.UseGuidePageText.Text4.txt");
            MainImage4.Source = AssortedUtil.GetEmbeddedImage(typeof(UsageGuidePage), "CosmicWatch.Resources.Images.UseGuidePageImages.Image4.png");
            
        }
        //[Toolbar Icons]
        private void InitializeToolbarIcons()
        {
            int iconCount = 0;
            foreach (ToolbarItem item in ToolbarItems)
            {
                item.IconImageSource = AssortedUtil.GetEmbeddedImage(typeof(MainPage), "CosmicWatch.Resources.Icons.UseGuidePageToolbarIcons.Icon" + iconCount.ToString() + ".png");
                iconCount++;
            }
        }
        //[Navigation Bar Buttons]
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