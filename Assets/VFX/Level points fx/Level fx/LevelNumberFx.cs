using UnityEngine;
using UnityEngine.UI;

public class LevelNumberFx : MonoBehaviour
{
    public Text text;

    private void Awake()
    {
        text ??= GetComponent<Text>();
    }

    private void Start()
    {
        References.levelManager.onStartLevel += UpdateText;
    }

    private void UpdateText()
    {
        text.text = "Level " + References.levelManager.levelNumber;
    }
}