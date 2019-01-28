using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class ArcherUpgradeUI : MonoBehaviour
{
    public int MaxTargetsNow;
    public int MaxTargetsDifference;
    public int MaxTargetsAfter;

    public float SlowDownFactorNow;
    public float SlowDownFactorDifference;
    public float SlowDownFactorAfter;

    public float RangeNow;
    public float RangeDifference;
    public float RangeAfter;

    public int DamageNow;
    public int DamageDifference;
    public int DamageAfter;

    public float CooldownNow;
    public float CooldownDifference;
    public float CooldownAfter;

    public bool isUpgradeable=true;

    public Transform Statistics;
    // Start is called before the first frame update
    void Start()
    {
        Statistics.Find("MaxTargetsNow").GetComponent<TextMeshProUGUI>().text = String.Empty;
        Statistics.Find("MaxTargetsAfter").GetComponent<TextMeshProUGUI>().text = String.Empty;


        Statistics.Find("DamageNow").GetComponent<TextMeshProUGUI>().text = String.Empty;
        Statistics.Find("DamageAfter").GetComponent<TextMeshProUGUI>().text = String.Empty;


        Statistics.Find("RangeNow").GetComponent<TextMeshProUGUI>().text = String.Empty;
        Statistics.Find("RangeAfter").GetComponent<TextMeshProUGUI>().text = String.Empty;


        Statistics.Find("SlowNow").GetComponent<TextMeshProUGUI>().text = String.Empty;
        Statistics.Find("SlowAfter").GetComponent<TextMeshProUGUI>().text = String.Empty;

        Statistics.Find("CooldownNow").GetComponent<TextMeshProUGUI>().text = String.Empty;
        Statistics.Find("CooldownAfter").GetComponent<TextMeshProUGUI>().text = String.Empty;

        RectTransform backgroundTransform = GetComponentInParent<RectTransform>();
        backgroundTransform.position = new Vector3(backgroundTransform.rect.size.x / 4, backgroundTransform.rect.size.y / 4) +
                                       Input.mousePosition + new Vector3(-190, 40);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetValues()
    {
        Statistics.Find("MaxTargetsNow").GetComponent<TextMeshProUGUI>().text = MaxTargetsNow.ToString();
        Statistics.Find("DamageNow").GetComponent<TextMeshProUGUI>().text = DamageNow.ToString();
        Statistics.Find("RangeNow").GetComponent<TextMeshProUGUI>().text = RangeNow.ToString(CultureInfo.InvariantCulture);
        Statistics.Find("SlowNow").GetComponent<TextMeshProUGUI>().text = SlowDownFactorNow.ToString(CultureInfo.InvariantCulture);
        Statistics.Find("CooldownNow").GetComponent<TextMeshProUGUI>().text = CooldownNow.ToString(CultureInfo.InvariantCulture);
        
        if (isUpgradeable)
        {
            Statistics.Find("MaxTargetsAfter").GetComponent<TextMeshProUGUI>().text = MaxTargetsAfter.ToString() + "(" + MaxTargetsDifference.ToString() + ")"; ;
            Statistics.Find("DamageAfter").GetComponent<TextMeshProUGUI>().text = DamageAfter.ToString() + "(" + DamageDifference.ToString() + ")";
            Statistics.Find("RangeAfter").GetComponent<TextMeshProUGUI>().text = RangeAfter.ToString(CultureInfo.InvariantCulture) + "(" + RangeDifference.ToString() + ")";
            Statistics.Find("SlowAfter").GetComponent<TextMeshProUGUI>().text = SlowDownFactorAfter.ToString(CultureInfo.InvariantCulture) + "(" + SlowDownFactorDifference.ToString() + ")";
            Statistics.Find("CooldownAfter").GetComponent<TextMeshProUGUI>().text = CooldownAfter.ToString(CultureInfo.InvariantCulture) + "(" + CooldownDifference.ToString() + ")";

        }
        else
        {
            Statistics.Find("TitleAfter").GetComponent<TextMeshProUGUI>().text = String.Empty;
            Statistics.Find("MaxTargetsAfter").GetComponent<TextMeshProUGUI>().text= String.Empty;
            Statistics.Find("DamageAfter").GetComponent<TextMeshProUGUI>().text = String.Empty;
            Statistics.Find("RangeAfter").GetComponent<TextMeshProUGUI>().text = String.Empty;
            Statistics.Find("SlowAfter").GetComponent<TextMeshProUGUI>().text = String.Empty;
            Statistics.Find("CooldownAfter").GetComponent<TextMeshProUGUI>().text= String.Empty;
        }
    }
}
