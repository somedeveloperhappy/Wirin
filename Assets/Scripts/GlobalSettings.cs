using UnityEngine;

static public class GlobalSettings
{
    public enum Quality
    {
        Low = 0, Medium = 1, High = 2
    }
    public enum Theme
    {
        flat
    }

    static Quality m_quality;
    static Theme m_theme;
    static public Quality quality => m_quality;
    static public Theme theme => m_theme;

    static public void LoadSettings()
    {
        m_quality = (Quality) int.Parse(PlayerPrefs.GetString("quality", "0"));
        m_theme = (Theme) int.Parse(PlayerPrefs.GetString("theme", "0"));
    }
    static public void SetQuality(Quality quality)
    {
        m_quality = quality;
        Debug.Log($"Quality set to {quality} ({(int)quality})");

        UnityEngine.QualitySettings.SetQualityLevel((int) quality, true);
        PlayerPrefs.SetString("quality", ((int) m_quality).ToString());
    }
    static public void SetTheme(Theme theme)
    {
        m_theme = theme;
        Debug.Log($"Theme set to {m_theme}");
        PlayerPrefs.SetString("theme", ((int) m_theme).ToString());
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Activate()
    {
        string val = PlayerPrefs.GetString("Quality", "0");
        int v = int.Parse(val);
        SetQuality((Quality) v);

        val = PlayerPrefs.GetString("Theme", "0");
        v = int.Parse(val);
        SetTheme((Theme) v);
    }
}