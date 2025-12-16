using UnityEngine;

public class ShowScreenManager : MonoBehaviour
{
    public void OpenWebView(string url)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

                // Создаем экземпляр CustomTabsIntent.Builder через конструктор
                using (AndroidJavaObject customTabsIntentBuilder = new AndroidJavaObject("androidx.browser.customtabs.CustomTabsIntent$Builder"))
                {
                    // Добавляем дополнительные параметры для минимизации видимости строки URL
                    customTabsIntentBuilder.Call<AndroidJavaObject>("addDefaultShareMenuItem");

                    // Настраиваем цвет панели инструментов
                    int color = new AndroidJavaClass("android.graphics.Color").CallStatic<int>("parseColor", "#000000"); // Черный цвет
                    customTabsIntentBuilder.Call<AndroidJavaObject>("setToolbarColor", color);

                    // Пытаемся скрыть строку URL (если возможно)
                    customTabsIntentBuilder.Call<AndroidJavaObject>("setUrlBarHidingEnabled", true);

                    // Настраиваем анимации перехода
                    AndroidJavaObject resources = currentActivity.Call<AndroidJavaObject>("getResources");
                    int enterAnimation = resources.Call<int>("getIdentifier", "fade_in", "anim", currentActivity.Call<string>("getPackageName"));
                    int exitAnimation = resources.Call<int>("getIdentifier", "fade_out", "anim", currentActivity.Call<string>("getPackageName"));
                    customTabsIntentBuilder.Call<AndroidJavaObject>("setExitAnimations", currentActivity, enterAnimation, exitAnimation);

                    AndroidJavaObject customTabsIntent = customTabsIntentBuilder.Call<AndroidJavaObject>("build");

                    // Создаем URI
                    using (AndroidJavaObject uri = new AndroidJavaClass("android.net.Uri").CallStatic<AndroidJavaObject>("parse", url))
                    {
                        // Запускаем Custom Tab
                        customTabsIntent.Call("launchUrl", currentActivity, uri);
                    }
                }
            }
        }
        else
        {
            Debug.Log("Custom Tabs are only supported on Android.");
        }
    }
}