using System;
using Xamarin.Essentials;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Android;
using Android.Content;


namespace Gallery_Module40.Droid
{
    [Activity(Label = "Gallery_Module40", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        const int RequestStoragePermissionCode = 1;

        internal static Context ActivityContext { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            

            ActivityContext = this;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            #region Perm

            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new[] { Manifest.Permission.ReadExternalStorage }, RequestStoragePermissionCode);
            }

            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new[] { Manifest.Permission.WriteExternalStorage }, RequestStoragePermissionCode);
            }

            #endregion Perm

            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            //Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            #region Perm
            if (requestCode == RequestStoragePermissionCode)
            {
                //!!! Тут постоянно получаю в grantResults[0] -1
                if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
                {
                    // Permission granted, proceed with your logic
                }
                else
                {
                    // Permission denied, handle accordingly
                }
            }
            #endregion Perm
        }
    }
}