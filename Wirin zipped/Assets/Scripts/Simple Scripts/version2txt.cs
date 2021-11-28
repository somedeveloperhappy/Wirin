using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class version2txt : MonoBehaviour
{
    public string text = "dev version %VER%";

    private void OnEnable() {
        GetComponent<UnityEngine.UI.Text>().text = text.Replace("%VER%", Application.version);
    }
}