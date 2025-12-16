using UnityEngine;

public class DeveloperModeCheck
{

    public static bool IsDeveloperModeEnabled()
    {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
        using (AndroidJavaObject contentResolver = currentActivity.Call<AndroidJavaObject>("getContentResolver"))
        using (AndroidJavaClass settingsSecure = new AndroidJavaClass("android.provider.Settings$Secure"))
        {
            int devMode = settingsSecure.CallStatic<int>("getInt", contentResolver, "development_settings_enabled", 0);
            return devMode == 1;
        }
    }
}