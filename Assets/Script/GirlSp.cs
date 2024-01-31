using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GirlSp : MonoBehaviour
{
    public Image girlSp;
    public Sprite[] girlTxt;
    public Slider bar;
    public Image barFill;
    public Color lowColor;
    public Color highColor;

    public void onMoodChange(float value)
    {
        bar.value = value;
        int lvl = Mathf.FloorToInt(value / 20f);
        lvl = Mathf.Clamp(lvl, 0, 4);
        girlSp.sprite = girlTxt[lvl];

        barFill.color = new Color(lowColor.r + ((highColor.r - lowColor.r) * value / bar.maxValue), lowColor.g + ((highColor.g - lowColor.g) * value / bar.maxValue), lowColor.b + ((highColor.b - lowColor.b) * value / bar.maxValue));
    }
}
