using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IdentifyFaces
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
            (MainPage as NavigationPage).BarBackgroundColor = Color.FromRgb(00,159,204);

        }

        protected override void OnStart()
        {
            // Handle when your app starts
            
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
