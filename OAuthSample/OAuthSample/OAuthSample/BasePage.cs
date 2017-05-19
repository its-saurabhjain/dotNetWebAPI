using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace OAuthSample
{
    public class BasePage : ContentPage
    {
        protected override void OnAppearing() // It will start immediately after the screen is appeared
        {
            base.OnAppearing();

            if (!App.IsLoggedIn)
            {
                Navigation.PushModalAsync(new LoginPage());
            }
        }
    }
}
