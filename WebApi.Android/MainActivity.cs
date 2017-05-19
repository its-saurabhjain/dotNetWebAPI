using Android.App;
using Android.Widget;
using Android.OS;
using Xamarin.Auth;
using Android.Content;
using System;
using System.Threading.Tasks;

namespace WebApi.Android
{
    [Activity(Label = "WebApi.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);



            // Set our view from the "main" layout resourceSetContentView (Resource.Layout.Main);
            var authenticator = new OAuth2Authenticator(
                "socialnetwork_implicit", "read",
                new System.Uri("http://localhost:15638/connect/authorize"),
                new System.Uri("http://localhost:28307/"));
                var result = await this.LoginAsync(true, authenticator);

        }
        public async Task<Account> LoginAsync(bool allowCancel, OAuth2Authenticator auth)
        {

            // If authorization succeeds or is canceled, .Completed will be fired.  
            var tcs1 = new TaskCompletionSource<AuthenticatorCompletedEventArgs>();
            EventHandler<AuthenticatorCompletedEventArgs> d1 =
            (o, e) =>
            {
                try
                {
                    tcs1.TrySetResult(e);
                }
                catch (Exception ex)
                {
                    tcs1.TrySetResult(new AuthenticatorCompletedEventArgs(null));
                }
            };
            try
            {
                auth.Completed += d1;
                var intent = auth.GetUI(Activity2) as Intent;
                activity.StartActivity(intent);
                var result = await tcs1.Task;
                return result.Account;
            }
            catch (Exception)
            {
                // todo you should handle the exception  
                return null;
            }
            finally
            {
                auth.Completed -= d1;
            }
        }

    }

 }

