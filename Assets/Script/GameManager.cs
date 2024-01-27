using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using TMPro;

public enum gameoverReason
{
    Winning, Low, High
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public double maxTime = 60;
    public int scoreRange = 5;
    public EndConfirm confirm;
    public Transform ballStartPos;
    public GameObject ballPrefab;
    public TextMeshProUGUI scoreText;
    public GirlSp girlsp;

    [HideInInspector]
    public double timeCounter = 0;
    public int score
    {
        get => _score;
        set
        {
            _score = value;
            scoreText.text = "score:" + value.ToString();
        }
    }
    public float mood
    {
        get => _mood;
        set
        {
            _mood = value;
            this.girlsp.onMoodChange(value);
        }
    }
    public float power
    {
        get => _power;
        set
        {
            _power = value;
        }
    }
    public ObjectPool<GameObject> ballObjPool;
    private int _score = 0;
    private float _mood = 50;
    private float _power = 100;
    private bool isPause = false;

    void Awake() {
        if (!GameManager.instance)
        {
            GameManager.instance = this;
        }
       
        ballObjPool = new ObjectPool<GameObject>(create, getObj, release);
    }
    
    void Start()
    {
        Reset();
    }

    void Reset() {
        isPause = false;
        timeCounter = maxTime;
        score = 0;
        power = 100;
        mood = 50;
        confirm.gameObject.SetActive(false);
    }

    void GameOver(gameoverReason reason) {
        Debug.Log("Game Over");
        isPause = true;
        confirm.showEndConfirm(reason, score);
    }

    private void randomShootBall()
    {
        if (Random.Range(0f, 1f) > 0.1)
        {
            return;
        }

        int ballscore = Random.Range(-scoreRange, scoreRange + 1);
        GameObject ball = ballObjPool.Get();
        ball.GetComponent<Ball>().score = ballscore;
    }

    void Update()
    {
        if (isPause) {
            return;
        }

        if (Input.GetMouseButtonDown(0) && power > 10)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//world
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            if (hit) {
                ballObjPool.Release(hit.collider.gameObject);
                power -= 10;
            }
        }

        power += Time.deltaTime;

        randomShootBall();

        timeCounter -= Time.deltaTime;
        if (timeCounter <= 0)
        {
            GameOver(gameoverReason.Winning);
        }
        else if (mood >= 100)
        {
            GameOver(gameoverReason.High);
        }
        else if (mood <= 0)
        {
            GameOver(gameoverReason.Low);
        }
    }

    public void onClickAgain() {
        Reset();
    }

    private GameObject create()
    {
        GameObject result = Instantiate(ballPrefab);
        return result;
    }

    private void release(GameObject obj)
    {
        obj.SetActive(false);
    }

    private void getObj(GameObject obj)
    {
        obj.SetActive(true);
        obj.transform.SetParent(ballStartPos, false);
        obj.transform.localPosition = new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(-1f, 1f), 0);
    }
}
