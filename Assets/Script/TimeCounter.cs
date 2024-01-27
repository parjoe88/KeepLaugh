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
    void Start()
    {
        timeText = gameObject.GetComponent<TextMeshProUGUI>();
        float z = -transform.eulerAngles.z;
        transform.DOLocalRotate(new Vector3(0,0,z), 1.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        timeText.text = timeToText(GameManager.instance.timeCounter);
    }
    private string timeToText(double t)
    {
        DateTime startTime = new DateTime(2024, 1, 27, 8, 0, 0);
        string str = startTime.AddMinutes(-t * 10).ToString("hh:mm tt", CultureInfo.InvariantCulture);
        return str;
    }
}
