using UnityEngine;

namespace ThemeSystem
{
    [CreateAssetMenu(fileName = "ThemeManager", menuName = "Wirin/ThemeManager", order = 0)]
    public class ThemeManager : ScriptableObject
    {
        public Theme[] m_themes;
        static public Theme[] themes;

        [SerializeField] int m_enabledIndex = -1;
        static int _enabledIndex = -1;
        static public int enabledIndex
        {
            get
            {
                if (_enabledIndex != -1) return _enabledIndex;
                // else load it from player prefs
                throw new System.Exception("Enabled Index not found!");
            }
        }


        static public void SetEnabledIndex(int value)
        {
            if (value < 0) throw new System.Exception("Enabled Index should not be less than zero!");
            _enabledIndex = value;
            SaveStatic();
        }


        private void OnEnable()
        {
            Load();
            AssingStatics();
        }

        [ContextMenu("Assign Statics")]
        public void AssingStatics()
        {
            // setting up static values
            themes = m_themes;
            SetEnabledIndex(m_enabledIndex);
            Debug.Log("theme manager loaded");
        }

        [ContextMenu("Load")]
        public void Load()
        {
            var pref_val = PlayerPrefs.GetString("themes", string.Empty);
            if (pref_val == string.Empty)
            {
                // first time. save it instead of loading it
                Debug.Log("No prefs about theme. Creating one...");
                Save();
                return;
            }
            var pref_vals = pref_val.Split(new string[] { "__" }, System.StringSplitOptions.None);

            var codes = pref_vals[0].ToCharArray();

            if (codes.Length != m_themes.Length) throw new System.Exception("Theme count is different than what's been saved before. re-save the themes");

            for (int i = 0; i < codes.Length; i++)
                m_themes[i].owned = codes[i] == '1';

            if (pref_vals.Length > 1)
                m_enabledIndex = int.Parse(pref_vals[1]);
        }
        static void SaveStatic()
        {
            string codes = string.Empty;
            for (int i = 0; i < themes.Length; i++) codes += themes[i].owned ? "1" : "0";
            codes += "__" + _enabledIndex;
            PlayerPrefs.SetString("themes", codes);
        }

        [ContextMenu("Save")]
        public void Save()
        {
            string codes = string.Empty;
            for (int i = 0; i < m_themes.Length; i++) codes += m_themes[i].owned ? "1" : "0";
            codes += "__" + m_enabledIndex;
            PlayerPrefs.SetString("themes", codes);
        }
    }
}
