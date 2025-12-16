using System.Globalization;

public class DeviceLanguageHelper
{
    public static string GetLanguageISO()
    {
        string systemLang = CultureInfo.CurrentCulture.Name;

        try
        {
            CultureInfo culture = new CultureInfo(systemLang);
            return culture.TwoLetterISOLanguageName.ToUpper();
        }
        catch
        {
            return "EN";
        }
    }
}