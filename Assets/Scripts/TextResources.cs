using UnityEngine;
using UnityEngine.UI;

public class TextResources : MonoBehaviour {
    public Text countResources;
	void Start () {
        countResources = GetComponent<Text>();
	}
	
	void Update () {
        countResources.text = "Resources : " +  Enemies.resources.ToString();
	}
}
