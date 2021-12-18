using UnityEngine;
using UnityEngine.UI;


namespace SimpleScripts
{
    [ExecuteAlways]
    public class version2txt : MonoBehaviour
    {
        public string format = "dev version %VER%";
        public TMPro.TMP_Text text;

        private void OnEnable()
        {
            text.text = format.Replace("%VER%", Application.version);
        }
    }
}
