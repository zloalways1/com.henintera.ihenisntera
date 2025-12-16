using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class PreBuildSceneCreator : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;
    private string productName = "";

    public void OnPreprocessBuild(BuildReport report)
    {
        PlayerSettings.SplashScreen.show = true;
        PlayerSettings.SplashScreen.backgroundColor = Color.black;
        PlayerSettings.SplashScreen.animationMode = PlayerSettings.SplashScreen.AnimationMode.Static;
        Debug.Log("Splash Screen set to black with static animation.");
		
		PlayerSettings.Android.minifyDebug = false;
		PlayerSettings.Android.minifyRelease = false;
		//PlayerSettings.Android.minifyWithR8 = false;
        Debug.Log("Minify (R8/Proguard) disabled for both Release and Debug builds.");

        PlayerSettings.productName = productName;
        Debug.Log($"Product name set to {productName}");
    }
}