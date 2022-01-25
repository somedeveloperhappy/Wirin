using UnityEngine;

public class ShowSettings : MonoBehaviour
{
    public void displaySettings()
    {
        GameCreator.instance.settingsMenu.enabled = true;
    }
}