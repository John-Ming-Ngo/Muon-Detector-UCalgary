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
    public partial class CreditsPage : ContentPage
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
        public CreditsPage()
        {
            InitializeComponent();

            MainTitle = AssortedUtil.GetEmbeddedText(typeof(CreditsPage), "CosmicWatch.Resources.Text.CreditsPageText.MainTitle.txt");
            MainText = AssortedUtil.GetEmbeddedText(typeof(CreditsPage), "CosmicWatch.Resources.Text.CreditsPageText.MainText.txt");
        }
    }
}