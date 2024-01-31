using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TimeCounter : MonoBehaviour
{
    private TextMeshProUGUI timeText;
    private AudioSource timeAs;
    private float lastTime;
    void Start()
    {
        timeAs = gameObject.GetComponent<AudioSource>();
        timeText = gameObject.GetComponent<TextMeshProUGUI>();
        float z = -transform.eulerAngles.z;
        transform.DOLocalRotate(new Vector3(0,0,z), 1.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    void Update()
    {
        float curTime = GameManager.instance.timeCounter;
        timeText.text = timeToText(curTime);
        if (curTime < 3 && lastTime > 3)
        {
            Debug.Log("倒數3秒");
            timeAs.PlayOneShot(timeAs.clip);
        }
        lastTime = curTime;
    }
    private string timeToText(double t)
    {
        DateTime startTime = new DateTime(2024, 1, 27, 22, 0, 0);
        string str = startTime.AddMinutes(-t * 14).ToString("hh:mm tt", CultureInfo.CreateSpecificCulture("en-us"));
        return str;
    }
}
