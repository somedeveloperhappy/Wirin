using System;
using TMPro;
using UnityEngine;
public class SubmitScoreMenuFunctions : MonoBehaviour
{
    decimal score;
    public TMP_Text scoreTxt;
    public TMP_InputField nameInput;


    [System.Serializable]
    public struct HideSettings
    {
        public Animator anim;
        public string anim_name;
        public float anim_duration;
        public UnityEngine.UI.GraphicRaycaster raycaster;
        public CanvasSystem.CanvasBase canvasBase;
    }
    public HideSettings hideSettings;

    private void OnEnable() 
    {
        SetScore(Management.GameController.lastGameScore);
    }
    public void SetScore(decimal score)
    {
        this.score = score;
        scoreTxt.text = score.ToString();
    }

    public void Submit()
    {
        StartCoroutine(SubmitAsync());
    }

    private System.Collections.IEnumerator SubmitAsync()
    {
        // save score
        string name = nameInput.text;
        if (name == string.Empty) name = "unknown";
        References.highScoreManager.SaveScore(name, score);
        yield return null;

        // disable functionalities
        hideSettings.raycaster.enabled = false;

        // hide anim
        hideSettings.anim.Play(hideSettings.anim_name);
        yield return new WaitForSecondsRealtime(hideSettings.anim_duration);

        // absolute
        hideSettings.canvasBase.enabled = false;

        // restore defaults
        hideSettings.raycaster.enabled = true;
    }

    public void Cancell()
    {
        StartCoroutine(CancellAsync());
    }
    public System.Collections.IEnumerator CancellAsync()
    {
        // disable functionalities
        hideSettings.raycaster.enabled = false;

        // hide anim
        hideSettings.anim.Play(hideSettings.anim_name);
        yield return new WaitForSecondsRealtime(hideSettings.anim_duration);

        // absolute
        hideSettings.canvasBase.enabled = false;

        // restore defaults
        hideSettings.raycaster.enabled = true;
    }
}