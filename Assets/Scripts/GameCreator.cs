using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCreator : MonoBehaviour
{
    public ThemeSystem.ThemeBased<string> sceneName;
    
    private void Start() 
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            sceneName
        );
    }
}
