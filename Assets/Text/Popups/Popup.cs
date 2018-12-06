using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour {
    public Animator animator;
    private Text PopupText;

	void Start () {
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        PopupText = animator.GetComponent<Text>();
        Destroy(gameObject, clipInfo[0].clip.length);
    }

    public void SetText(string newText)
    {
        PopupText = animator.GetComponent<Text>();
        PopupText.text = newText;
    }

    public void SetText(string newText, Color color)
    {
        PopupText = animator.GetComponent<Text>();
        PopupText.text = newText;
        PopupText.color = color;
    }
}
