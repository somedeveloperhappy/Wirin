using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public new InputField name, score;

    public void DoIt()
    {
        string name = this.name.text;
        decimal score = decimal.Parse(this.score.text);
        References.highScoreManager.SaveScore(name, score);
        var str = References.highScoreManager.GetLocalHighscore();
        string s = "scors : \n";
        foreach (var item in str)
        {
            s += item.name + " : " + item.score + "\n";
        }
        Debug.Log($"{s}");
    }
}