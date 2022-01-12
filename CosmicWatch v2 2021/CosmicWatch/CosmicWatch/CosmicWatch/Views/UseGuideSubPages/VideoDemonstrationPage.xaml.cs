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
    public partial class VideoDemonstrationPage : ContentPage
    {
        public VideoDemonstrationPage()
        {
            InitializeComponent();
            //[Initialize Page Display Variables]
            MainTextTitle.Text = AssortedUtil.GetEmbeddedText(typeof(VideoDemonstrationPage), "CosmicWatch.Resources.Text.UseGuideSubPagesText.VideoDemonstrationPageText.MainTitle.txt");
            MainTextDisplay.Text = AssortedUtil.GetEmbeddedText(typeof(VideoDemonstrationPage), "CosmicWatch.Resources.Text.UseGuideSubPagesText.VideoDemonstrationPageText.Text0.txt");
            WebDisplay.Source = "https://www.youtube.com/watch?v=OZBpu67tbS0";
            
        }
    }
}