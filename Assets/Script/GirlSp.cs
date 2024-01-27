using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GirlSp : MonoBehaviour
{
    public Image girlSp;
    public Sprite[] girlTxt;
    public Slider bar;

    public void onMoodChange(float value)
    {
        bar.value = value;
        int lvl = Mathf.FloorToInt(value / 20f);
        lvl = Mathf.Clamp(lvl, 0, 4);
        girlSp.sprite = girlTxt[lvl];
    }
}
