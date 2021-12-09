using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CosmicWatch
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            //Important: Setting this to NavigationPage to allow for navigation!
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
