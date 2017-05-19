using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace OAuthSample
{
    public class ProfilePage : BasePage //It is to ensure nothing to display before logging in.
    {
        public ProfilePage()
        {
            Content = new Label()
            {
                Text = "Profile Page",
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
        }
    }
}
