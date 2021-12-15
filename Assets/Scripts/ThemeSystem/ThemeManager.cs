using UnityEngine;

namespace ThemeSystem
{
    [ExecuteAlways]
    [CreateAssetMenu(fileName = "ThemeManager", menuName = "Wirin/ThemeManager", order = 0)]
    public class ThemeManager : ScriptableObject
    {
        static public ThemeManager instance;

        private void OnEnable()
        {
            Debug.Log($"theme manager activated");
            instance = this;
        }

        public Theme[] themes;
        public int enabledIndex = 0;

        [ContextMenu("Load")]
        public void Load()
        {
            var codes = PlayerPrefs.GetString("themes", string.Empty).ToCharArray();
            if (codes.Length != themes.Length) return;

            for (int i = 0; i < codes.Length; i++)
                themes[i].owned = codes[i] == '1';
        }

        [ContextMenu("Save")]
        public void Save()
        {
            string codes = string.Empty;
            for (int i = 0; i < themes.Length; i++) codes += themes[i].owned ? "1" : "0";
            PlayerPrefs.SetString("themes", codes);
        }
    }
}
