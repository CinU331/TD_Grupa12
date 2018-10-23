﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OverlayUI : MonoBehaviour {

	public TextMeshProUGUI resourcesCount;
	// Use this for initialization
	void Start () {
		resourcesCount = GetComponent<TextMeshProUGUI>();
	}
	
	// Update is called once per frame
	void Update () {
		resourcesCount.text = Enemies.resources.ToString();
	}
}