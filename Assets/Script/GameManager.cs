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
    public float maxTime = 60;
    public int scoreRange = 5;
    public EndConfirm confirm;
    public Transform ballStartPos;
    public GameObject ballPrefab;
    public TextMeshProUGUI scoreText;
    public GirlSp girlsp;
    public Slider powerBar;
    public Slider timeBar;
    public GameObject touchFx;
    public float BallNextTimeMax = 1f;
    public float BallNextTimeMin = 0.1f;
    public float recoverPower = 3;
    public float powerCostPerTouch= 10;

    [HideInInspector]
    public float timeCounter = 0;
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
            this.powerBar.value = value;
        }
    }
    public ObjectPool<GameObject> ballObjPool;
    private int _score = 0;
    private float _mood = 50;
    private float _power = 100;
    private bool isPause = false;
    private float nextGenTime = 0;

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
        nextGenTime -= Time.deltaTime;
        if (nextGenTime <= 0)
        {
            int ballscore = Random.Range(-scoreRange, scoreRange + 1);
            GameObject ball = ballObjPool.Get();
            ball.GetComponent<Ball>().score = ballscore;
            nextGenTime += Random.Range(BallNextTimeMin, BallNextTimeMax);
        }
    }

    void Update()
    {
        if (isPause) {
            return;
        }

        if (Input.GetMouseButtonDown(0) && power > powerCostPerTouch)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            if (hit)
            {
                Instantiate(touchFx).transform.position = hit.collider.gameObject.transform.position;
                score += Mathf.Abs(hit.collider.GetComponent<Ball>().score);
                ballObjPool.Release(hit.collider.gameObject);
                power -= powerCostPerTouch;
            }
        }

        power += Time.deltaTime * recoverPower;

        randomShootBall();

        timeCounter -= Time.deltaTime;
        timeBar.value = timeCounter / maxTime;
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

    public void onClickExit()
    {
        Application.Quit();
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
