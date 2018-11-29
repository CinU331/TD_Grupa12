using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameResources : MonoBehaviour {

	public delegate void CountChangedEvent();
	public event CountChangedEvent CreditsChanged;
	public event CountChangedEvent ResourcesChanged;

    private int mResources = 5;
    private int mCredits = 50;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public int Resources 
	{
		get
		{
			return mResources;
		}
		set
		{
			mResources = value;
			CreditsChanged();
		}
	}

	public int Credits 
	{
		get
		{
			return mCredits;
		}
		set
		{
			mCredits = value;
			ResourcesChanged();
		}
	}

	public void ChangeCreditsCount(int creditsDelta) 
	{
		mCredits += creditsDelta;
		CreditsChanged();
	}

	public void ChangeResourceCount(int resourceDelta) 
	{
		mResources += resourceDelta;
		ResourcesChanged();

        if (mResources <= 0)
        {
            LoadEndGameScreen();
        }
	}
	
    private void LoadEndGameScreen() 
    {
        SceneManager.LoadScene("GameOver");
    }
}
