using UnityEngine;
using UnityEngine.SceneManagement;

public class GameResources : MonoBehaviour {

	public delegate void CountChangedEvent();
	public event CountChangedEvent CreditsChanged;
	public event CountChangedEvent ResourcesChanged;
	
	
	public event CountChangedEvent TrapsChanged;

    private int mResources = 5;
    private int mCredits = 60;
	private int mSpikeTraps = 1;
	private int mSplashTraps = 1;
	private int mMolotovs = 2;

	public int SpikeTraps
	{
		get { return mSpikeTraps; }
		set
		{
			mSpikeTraps = value;
			TrapsChanged();
		}
	}

	public int SplashTraps
	{
		get { return mSplashTraps; }
		set
		{
			mSplashTraps = value;
			TrapsChanged();
		}
	}

    public int Molotovs
    {
        get { return mMolotovs; }
        set
        {
            mMolotovs = value;
            TrapsChanged();
        }
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
