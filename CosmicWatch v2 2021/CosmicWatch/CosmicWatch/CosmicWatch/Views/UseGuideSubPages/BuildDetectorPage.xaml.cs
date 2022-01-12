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
        public BuildDetectorPage()
        {
            InitializeComponent();
            //[Initialize Page Display Variables]
            MainTextTitle.Text = AssortedUtil.GetEmbeddedText(typeof(BuildDetectorPage), "CosmicWatch.Resources.Text.UseGuideSubPagesText.BuildDetectorPageText.MainTitle.txt");
            //[Text and Images: Row 1]
            MainTextDisplay.Text = AssortedUtil.GetEmbeddedText(typeof(BuildDetectorPage), "CosmicWatch.Resources.Text.UseGuideSubPagesText.BuildDetectorPageText.Text0.txt");
            MainImage.Source = AssortedUtil.GetEmbeddedImage(typeof(CreditsPage), "CosmicWatch.Resources.Images.UseGuideSubPagesImages.BuildDetectorPageImages.Image0.png");
            //[Text and Images: Row 2]
            MainTextDisplay1.Text = AssortedUtil.GetEmbeddedText(typeof(BuildDetectorPage), "CosmicWatch.Resources.Text.UseGuideSubPagesText.BuildDetectorPageText.Text1.txt");
            MainImage1.Source = AssortedUtil.GetEmbeddedImage(typeof(CreditsPage), "CosmicWatch.Resources.Images.UseGuideSubPagesImages.BuildDetectorPageImages.Image1.png");
            //[Text and Images: Row 3]
            MainTextDisplay2.Text = AssortedUtil.GetEmbeddedText(typeof(BuildDetectorPage), "CosmicWatch.Resources.Text.UseGuideSubPagesText.BuildDetectorPageText.Text2.txt");
            MainImage2.Source = AssortedUtil.GetEmbeddedImage(typeof(CreditsPage), "CosmicWatch.Resources.Images.UseGuideSubPagesImages.BuildDetectorPageImages.Image2.png");
            //[Text and Images: Row 4]
            MainTextDisplay3.Text = AssortedUtil.GetEmbeddedText(typeof(BuildDetectorPage), "CosmicWatch.Resources.Text.UseGuideSubPagesText.BuildDetectorPageText.Text3.txt");
            MainImage3.Source = AssortedUtil.GetEmbeddedImage(typeof(CreditsPage), "CosmicWatch.Resources.Images.UseGuideSubPagesImages.BuildDetectorPageImages.Image3.png");
            //[Text and Images: Row 5]
            MainTextDisplay4.Text = AssortedUtil.GetEmbeddedText(typeof(BuildDetectorPage), "CosmicWatch.Resources.Text.UseGuideSubPagesText.BuildDetectorPageText.Text4.txt");
            MainImage4.Source = AssortedUtil.GetEmbeddedImage(typeof(CreditsPage), "CosmicWatch.Resources.Images.UseGuideSubPagesImages.BuildDetectorPageImages.Image4.png");
        }
    }
}