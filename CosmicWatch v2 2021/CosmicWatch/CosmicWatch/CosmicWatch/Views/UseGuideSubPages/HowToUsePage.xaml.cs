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
    public partial class HowToUsePage : ContentPage
    {
        public HowToUsePage()
        {
            InitializeComponent();
            //[Initialize Page Display Variables]
            MainTextTitle.Text = AssortedUtil.GetEmbeddedText(typeof(HowToUsePage), "CosmicWatch.Resources.Text.UseGuideSubPagesText.HowToUsePageText.MainTitle.txt");
            //[Text and Images: Row 1]
            MainTextDisplay.Text = AssortedUtil.GetEmbeddedText(typeof(HowToUsePage), "CosmicWatch.Resources.Text.UseGuideSubPagesText.HowToUsePageText.Text0.txt");
            MainImage.Source = AssortedUtil.GetEmbeddedImage(typeof(HowToUsePage), "CosmicWatch.Resources.Images.UseGuideSubPagesImages.HowToUsePageImages.Image0.png");
            //[Text and Images: Row 2]
            MainTextDisplay1.Text = AssortedUtil.GetEmbeddedText(typeof(HowToUsePage), "CosmicWatch.Resources.Text.UseGuideSubPagesText.HowToUsePageText.Text1.txt");
            MainImage1.Source = AssortedUtil.GetEmbeddedImage(typeof(HowToUsePage), "CosmicWatch.Resources.Images.UseGuideSubPagesImages.HowToUsePageImages.Image1.png");
            //[Text and Images: Row 3]
            MainTextDisplay2.Text = AssortedUtil.GetEmbeddedText(typeof(HowToUsePage), "CosmicWatch.Resources.Text.UseGuideSubPagesText.HowToUsePageText.Text2.txt");
            MainImage2.Source = AssortedUtil.GetEmbeddedImage(typeof(HowToUsePage), "CosmicWatch.Resources.Images.UseGuideSubPagesImages.HowToUsePageImages.Image2.png");
            //[Text and Images: Row 4]
            MainTextDisplay3.Text = AssortedUtil.GetEmbeddedText(typeof(HowToUsePage), "CosmicWatch.Resources.Text.UseGuideSubPagesText.HowToUsePageText.Text3.txt");
            MainImage3.Source = AssortedUtil.GetEmbeddedImage(typeof(HowToUsePage), "CosmicWatch.Resources.Images.UseGuideSubPagesImages.HowToUsePageImages.Image3.png");
            //[Text and Images: Row 5]
            MainTextDisplay4.Text = AssortedUtil.GetEmbeddedText(typeof(HowToUsePage), "CosmicWatch.Resources.Text.UseGuideSubPagesText.HowToUsePageText.Text4.txt");
            MainImage4.Source = AssortedUtil.GetEmbeddedImage(typeof(HowToUsePage), "CosmicWatch.Resources.Images.UseGuideSubPagesImages.HowToUsePageImages.Image4.png");
        }
    }
}