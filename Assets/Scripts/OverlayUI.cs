using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OverlayUI : MonoBehaviour {

	public TextMeshProUGUI resourcesCount;
    public TextMeshProUGUI wavesCount;
	// Use this for initialization
	void Start () {
		resourcesCount = GetComponent<TextMeshProUGUI>();
        wavesCount = GetComponent<TextMeshProUGUI>();
    }
	
	// Update is called once per frame
	void Update () {
		resourcesCount.text = "Resources: " + Enemies.resources.ToString();
        //wavesCount.text = "Wave:" + WaveSpawner.wave.ToString() + "/" + WaveSpawner.numberOfWaves.ToString();
	}
}
