using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshRate2txt : MonoBehaviour
{
    UnityEngine.UI.Text text;

    private void Awake() {
        text = GetComponent<UnityEngine.UI.Text> ();
    }

    private void Update() {
        text.text = (1 / Time.unscaledDeltaTime) + "fps";
    }
}
