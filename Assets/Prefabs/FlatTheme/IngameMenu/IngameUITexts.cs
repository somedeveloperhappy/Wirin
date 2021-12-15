using UnityEngine;

public class IngameUITexts : MonoBehaviour, CanvasSystem.IOnCanvasEnabled
{

    #region references
    public TMPro.TMP_Text levelText;
    public Management.LevelManager levelManager;
    #endregion

    public string levelFormat = @"Level %LVL";

    [ContextMenu("Auto Resolve")]
    private void AutoResolve()
    {
        levelManager = FindObjectOfType<Management.LevelManager>();
        levelText = GetComponent<TMPro.TMP_Text>();
    }
    public void OnCanvasEnable()
    {
        levelText.text = levelFormat.Replace(@"%LVL", levelManager.levelNumber.ToString());
    }
}
