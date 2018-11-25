using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public Image ImgHealthBar;

    public Text TxtHealth;

    public float Max;

    private float mCurrentValue;

    private float mCurrentPercent;
    
    public void SetValue(float health)
    {
        if(health != mCurrentValue)
        {
            mCurrentValue = health;
            mCurrentPercent = mCurrentValue / Max;

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
        get { return mCurrentValue;  }
    }

	// Use this for initialization
	void Start () {

	}

}
