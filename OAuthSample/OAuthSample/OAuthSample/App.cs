using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace OAuthSample
{
    public class App : Application
    {
        static NavigationPage _NavPage;
        public static Page GetMainPage()
        {
            var profilePage = new ProfilePage();

            _NavPage = new NavigationPage(profilePage);

            return _NavPage;
        }


        public static bool IsLoggedIn
        {
            get { return !string.IsNullOrWhiteSpace(_Token); }
        }
        
        #region To store token form auth services
        static string _Token;
        public static String Token
        {
            get { return _Token; }
        }

        public static void SaveToken (string token)
        {
            _Token = token;
            
        }
        #endregion

        public static Action SuccessfulLoginAction
        {
            get
            {
                return new Action(() =>
                {
                    _NavPage.Navigation.PopModalAsync();
                });
            }
        }

    }
}
