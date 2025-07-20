using Microsoft.Maui.ApplicationModel;

namespace Eagle
{
    public class PermissionsBluetoothScan : Permissions.BasePlatformPermission
    {
#if ANDROID
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
            new[]
            {
                (Android.Manifest.Permission.BluetoothScan, true),
                (Android.Manifest.Permission.BluetoothConnect, true)
            };
#endif
    }
}
