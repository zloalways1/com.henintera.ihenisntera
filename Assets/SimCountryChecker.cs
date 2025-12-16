using UnityEngine;

public class SimCountryChecker : MonoBehaviour
{
    public static string GetSimCountry()
    {
        if (Application.platform != RuntimePlatform.Android)
            return "not_sim";

        try
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            using (AndroidJavaObject telephonyManager = currentActivity.Call<AndroidJavaObject>("getSystemService", "phone"))
            {
                if (telephonyManager != null)
                {
                    string country = telephonyManager.Call<string>("getSimCountryIso");
                    return string.IsNullOrEmpty(country) ? "not_sim" : country.ToUpper();
                }
            }
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError("Error: " + e.Message);
        }

        return "not_sim";
    }
}