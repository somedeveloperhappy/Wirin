using UnityEngine;

namespace Statics
{
    static public class GlobalSettings
    {
        public enum Quality
        {
            Low, Medium, High
        }
        public enum Theme
        {
            flat
        }

        static Quality m_quality;
        static Theme m_theme;
        static public Quality quality => m_quality;
        static public Theme theme => m_theme;
        static public void SetQuality(Quality quality)
        {
            m_quality = quality;
            Debug.Log( $"Quality set to {(int) quality}" );
        }
        static public void SetTheme(Theme theme)
        {
            m_theme = theme;
            Debug.Log( $"Theme set to {m_theme}" );
        }

        [RuntimeInitializeOnLoadMethod( RuntimeInitializeLoadType.BeforeSceneLoad )]
        static void Activate()
        {
            string val = PlayerPrefs.GetString( "Quality", "0" );
            int v = int.Parse( val );
            SetQuality( (Quality) v );

            val = PlayerPrefs.GetString( "Theme", "0" );
            v = int.Parse( val );
            SetTheme( (Theme) v );
        }
    }
}