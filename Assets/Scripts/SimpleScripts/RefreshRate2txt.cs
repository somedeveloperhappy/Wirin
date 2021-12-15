using UnityEngine;
using UnityEngine.UI;

namespace SimpleScripts
{
    public class RefreshRate2txt : MonoBehaviour
    {
        public string format = "$MS ms | $FPS fps";
        private Text text;

        public bool lockTo60 = true;

        private void Awake() => text = GetComponent<Text>();

        private void Start()
        {
            if (lockTo60)
                Application.targetFrameRate = 60;
        }

        int frames = 0;
        float t = 0;
        int fps = -1;

        private void Update()
        {
            frames++;
            t += Time.unscaledDeltaTime;
            if (t >= 1)
            {
                fps = frames;
                t = 0;
                frames = 0;
            }

            text.text = format.Replace(
                @"$MS", (1 / Time.unscaledDeltaTime).ToString()).Replace(
                @"$FPS", fps.ToString()).Replace(
                @"\t", "\t"
                ).Replace(
                @"\n", "\n"
                );
        }
    }
}
