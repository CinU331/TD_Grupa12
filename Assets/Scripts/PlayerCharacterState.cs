using Invector.CharacterController;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCharacterState : MonoBehaviour {

	public HUD Hud;
	public float HealthRestoreCooldown = 5f;
	public float RegenRatePerSecond = 10f;
	private float timeSinceLastDamage = 0;

	public float MaxHealthPoints = 100.0f;
	private float currentHealthPoints;

	public float MaxEnergyPoints = 100.0f;
	private float currentEnergyPoints;
	private bool isAlive = true;

	private HealthBar mHealthBar;
    private HealthBar mEnergyBar;

	private Animator animator;

    public GameObject SpikeTrap;
    public GameObject SplashTrap;

    public System.Diagnostics.Stopwatch timer;
    void Start () {
        timer = new System.Diagnostics.Stopwatch();
        timer.Start();

		animator = GetComponent<Animator>();
        SpikeTrap.transform.localScale = new Vector3(7, 5, 7);
		InitHudBars();
	}
	
	void Update () {
		timeSinceLastDamage += Time.deltaTime;

		if (timeSinceLastDamage > HealthRestoreCooldown && currentHealthPoints < MaxHealthPoints)
		{
			currentHealthPoints += RegenRatePerSecond * Time.deltaTime;
			if (currentHealthPoints > MaxHealthPoints)
			{
				currentHealthPoints = MaxHealthPoints;
			}

			UpdatePlayerHealthBar();
		}

        if(Input.GetKeyDown(KeyCode.Space))
        {
            timer.Reset();
            timer.Start();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && ShopController.AvailableSpikeTraps != 0 && timer.ElapsedMilliseconds > 1000)
        {
            GameObject tmp = new GameObject();
            tmp.transform.position = new Vector3(transform.position.x, transform.position.y - 3.4f, transform.position.z);
            Instantiate(SpikeTrap, tmp.transform);
            ShopController.AvailableSpikeTraps--;

        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && ShopController.AvailableSplashTraps != 0  && timer.ElapsedMilliseconds > 1000)
        {
            GameObject tmp = new GameObject();
            tmp.transform.position = new Vector3(transform.position.x, transform.position.y + 0.02f, transform.position.z);
            Instantiate(SplashTrap, tmp.transform);
            ShopController.AvailableSplashTraps--;
        }
    }

	public void DealDamage(DamageParameters damageParameters)
    {
        currentHealthPoints -= damageParameters.damageAmount;

		timeSinceLastDamage = 0f;
        
        if (currentHealthPoints <= 0 && isAlive)
        {
            StartCoroutine(youDied());
        }

		if (currentHealthPoints < 0) 
		{
			currentHealthPoints = 0;
		}
		UpdatePlayerHealthBar();
    }

	private void InitHudBars() {
		mHealthBar = Hud.transform.Find("Bars_Panel/HealthBar").GetComponent<HealthBar>();
        mHealthBar.Max = MaxHealthPoints;
        currentHealthPoints = MaxHealthPoints;
        mHealthBar.SetValue(MaxHealthPoints);

        mEnergyBar = Hud.transform.Find("Bars_Panel/EnergyBar").GetComponent<HealthBar>();
        mEnergyBar.Max = MaxEnergyPoints;
        currentEnergyPoints = MaxEnergyPoints;
        mEnergyBar.SetValue(MaxEnergyPoints);
	}

	public IEnumerator youDied()
    {
        animator.SetTrigger("Death");
        isAlive = false;
        vThirdPersonMotor.lockMovement = true;
        yield return new WaitForSeconds(5f);
        Debug.Log("You died!");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
    }

	private void UpdatePlayerHealthBar()
	{
		mHealthBar.SetValue(currentHealthPoints);
	}
}
