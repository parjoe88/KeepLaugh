using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndConfirm : MonoBehaviour
{
    public GameObject win;
    public GameObject lose;
    public TextMeshProUGUI endScore;

    public void showEndConfirm(gameoverReason reason, int score) {
        gameObject.SetActive(true);
        if (reason == gameoverReason.Winning)
        {
            win.SetActive(true);
            lose.SetActive(false);
        }
        else
        {
            win.SetActive(false);
            lose.SetActive(true);
        }
        endScore.text = "score:" + score.ToString();
    }
}
