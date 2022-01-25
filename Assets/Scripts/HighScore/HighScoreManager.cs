using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace HighScore
{
    public class HighScoreManager : MonoBehaviour
    {
        public System.Action onLocalHighscoreGet;
        public System.Action onGlobalHighscoreGet;

        string local_highscore_filename = "score.txt";
        string path => Application.persistentDataPath + "/" + local_highscore_filename;


        public struct Score
        {
            public string name;
            public decimal score;
            static public string seperator = "_|_";
        }
        public List<Score> GetGlobalHighscore()
        {
            onGlobalHighscoreGet?.Invoke();
            return null;
        }

        public void SaveScore(string name, decimal score)
        {
            // get local scores and add the new score
            var scores = GetLocalHighscore();
            if (scores is null) scores = new List<Score>();

            Score newscore = new Score();
            newscore.name = name;
            newscore.score = score;
            scores.Add(newscore);
            SortRanks(ref scores);

            // save it
            if (!File.Exists(path)) File.Create(path);

            string[] str = new string[scores.Count];
            for (int i = 0; i < scores.Count; i++)
            {
                str[i] = scores[i].name + Score.seperator + scores[i].score;
            }
            File.WriteAllLines(path, str);
            Debug.Log($"score {name} : {score} added.");
        }

        public List<Score> GetLocalHighscore()
        {
            // get data from file
            if (!File.Exists(path)) return null;

            // turn strings into Scores
            List<Score> scores = new List<Score>();
            var lines = File.ReadAllLines(path);

            foreach (var line in lines)
            {
                try
                {
                    string[] str = line.Split(new string[] { Score.seperator }, System.StringSplitOptions.RemoveEmptyEntries);
                    scores.Add(new Score() { name = str[0], score = decimal.Parse(str[1]) });
                }
                catch { }
            }

            onLocalHighscoreGet?.Invoke();
            return scores;
        }

        private void SortRanks(ref List<Score> scores)
        {
            for (int i = 0; i < scores.Count - 1; i++)
            {
                for (int j = i + 1; j < scores.Count; j++)
                {
                    if (scores[j].score > scores[i].score)
                    {
                        // swap
                        var tmp = scores[i];
                        scores[i] = scores[j];
                        scores[j] = tmp;
                    }
                }
            }
        }
    }
}