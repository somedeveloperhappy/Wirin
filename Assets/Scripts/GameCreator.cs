using UnityEngine;
using UnityEngine.UI;

public class GameCreator : MonoBehaviour
{
    static public GameCreator instance;
    public int flatThemeSceneBuildIndex = 1;
    public CanvasSystem.CanvasBase settingsMenu;
    private void Start()
    {
        GlobalSettings.LoadSettings();
        
        var operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(flatThemeSceneBuildIndex, UnityEngine.SceneManagement.LoadSceneMode.Additive);

        operation.completed += _ => setNewSceneActive();

        if (instance) Destroy(instance);
        instance = this;
    }
    private void setNewSceneActive()
    {
        var success = UnityEngine.SceneManagement.SceneManager.SetActiveScene(
            UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(flatThemeSceneBuildIndex));
        Debug.Log($"set active ? {success}");
    }
}
