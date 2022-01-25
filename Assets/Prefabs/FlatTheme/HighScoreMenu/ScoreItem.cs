using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlatTheme.HighScoreMenu
{
    public class ScoreItem : MonoBehaviour
    {
        public UnityEngine.UI.RawImage background;
        public TMPro.TMP_Text nameTxt, scoreTxt;
        public Color col1, col2, col3, colRest;

        public void Init(string name, int rank, decimal score)
        {
            this.nameTxt.text = name;
            this.scoreTxt.text = score.ToString();

            if (rank == 1)
            {
                this.nameTxt.color = this.scoreTxt.color = this.background.color = col1;
            }
            else if (rank == 2)
            {
                this.nameTxt.color = this.scoreTxt.color = this.background.color = col2;
            }
            else if (rank == 3)
            {
                this.nameTxt.color = this.scoreTxt.color = this.background.color = col3;
            }
            else
            {
                this.nameTxt.color = this.scoreTxt.color = this.background.color = colRest;
            }
        }

        private void OnEnable() => gameObject.SetActive(true);
        private void OnDisable() => gameObject.SetActive(false);
    }
}