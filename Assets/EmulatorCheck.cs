using UnityEngine;

public class EmulatorCheck : MonoBehaviour
{
    public static bool IsRunningOnEmulator()
    {
        if (Application.platform != RuntimePlatform.Android)
            return false;

        using (AndroidJavaClass buildClass = new AndroidJavaClass("android.os.Build"))
        {
            string brand = buildClass.GetStatic<string>("BRAND");
            string device = buildClass.GetStatic<string>("DEVICE");
            string model = buildClass.GetStatic<string>("MODEL");
            string hardware = buildClass.GetStatic<string>("HARDWARE");
            string product = buildClass.GetStatic<string>("PRODUCT");
            string fingerprint = buildClass.GetStatic<string>("FINGERPRINT");

            if (brand.ToLower().Contains("generic") ||
                device.ToLower().Contains("generic") ||
                model.ToLower().Contains("sdk") ||
                product.ToLower().Contains("sdk") ||
                fingerprint.ToLower().Contains("generic") ||
                hardware.ToLower().Contains("goldfish") || hardware.ToLower().Contains("ranchu"))
            {
                return true;
            }
        }

        return false;
    }
}