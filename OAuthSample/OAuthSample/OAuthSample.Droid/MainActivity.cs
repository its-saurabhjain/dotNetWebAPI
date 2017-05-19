using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace OAuthSample.Droid
{
    [Activity(Label = "OAuthSample", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());

            Android.Telephony.TelephonyManager mTelephonyMgr;
            //Telephone Number
            mTelephonyMgr = (Android.Telephony.TelephonyManager)GetSystemService(TelephonyService);
            var PhoneNumber = mTelephonyMgr.Line1Number;
            //IMEI number
            String m_deviceId = mTelephonyMgr.DeviceId;
            
            //Android ID
            String m_androidId = Android.Provider.Settings.Secure.GetString(ContentResolver, Android.Provider.Settings.Secure.AndroidId);
                        
            //WLAN MAC Address            
            Android.Net.Wifi.WifiManager m_wm = (Android.Net.Wifi.WifiManager)GetSystemService(Android.Content.Context.WifiService);
            String m_wlanMacAdd = m_wm.ConnectionInfo.MacAddress;

            //Bluetooth Address
            Android.Bluetooth.BluetoothAdapter m_BluetoothAdapter = Android.Bluetooth.BluetoothAdapter.DefaultAdapter; 
            String m_bluetoothAdd = m_BluetoothAdapter.Address;



        }
    }
}

