using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public TextMeshPro scoreTxt;
    private int _score;
    public int score
    {
        get => _score;
        set
        {
            _score = value;
            scoreTxt.text = value >= 0 ? "+" + value.ToString() : value.ToString();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("btnWall"))
        {
            GameManager.instance.ballObjPool.Release(gameObject);
            GameManager.instance.mood += score;
            GameManager.instance.score += score;
;       }
    }

    public void OnEnable()
    {
        
    }

    public void OnDisable()
    {
        
    }

    public void FadeAway()
    {

    }
}
