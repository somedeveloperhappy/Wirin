using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelNumberFx : MonoBehaviour
{
    public UnityEngine.UI.Text text;

    private void Awake() {
        text ??= GetComponent<UnityEngine.UI.Text>();
    }

    private void Start() {
        References.levelManager.onStartLevel += UpdateText;
    }

    private void UpdateText() {
        text.text = "Level " + References.levelManager.levelNumber;
    }
}
