using UnityEngine;

public class EnableIfQualityWasRight : MonoBehaviour
{
    public GlobalSettings.Quality quality;
    public GameObject target;

    private void OnEnable()
    {
        Debug.Log($"quality is {GlobalSettings.quality} rigth now");
        target.SetActive((int) GlobalSettings.quality >= (int) quality);
    }
}