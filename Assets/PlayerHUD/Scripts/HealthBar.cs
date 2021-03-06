﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Image ImgHealthBar;

    public Text TxtHealth;

    public float Max;

    private float mCurrentValue;

    private float mCurrentPercent;

    public void SetValue(float health)
    {
        if (health != mCurrentValue)
        {
            mCurrentValue = health;
            mCurrentPercent = mCurrentValue / Max;
            ImgHealthBar.color = mCurrentPercent > 0.7f ? Color.green :
                mCurrentPercent > 0.5f ? new Color(214, 204, 0, 255) :
                mCurrentPercent > 0.3f ? new Color(1, 0.5f, 0, 1) : Color.red;
            TxtHealth.text = string.Format("{0} %", Mathf.RoundToInt(mCurrentPercent * 100));

            ImgHealthBar.fillAmount = mCurrentPercent;
        }
    }

    public float CurrentPercent
    {
        get { return mCurrentPercent; }
    }

    public float CurrentValue
    {
        get { return mCurrentValue; }
    }

    // Use this for initialization
    void Start()
    {

    }

}
