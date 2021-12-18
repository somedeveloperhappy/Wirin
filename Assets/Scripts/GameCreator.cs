using UnityEngine;
using UnityEngine.UI;

public class GameCreator : MonoBehaviour
{
    public ThemeSystem.ThemeManager themeManager;
    public ThemeSystem.ThemeBased<string> sceneName;
    public Text debugText;

    private void Start()
    {
        try
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        }
        catch (System.Exception e)
        {
            debugText.text = e.Message;
        }
    }
}
