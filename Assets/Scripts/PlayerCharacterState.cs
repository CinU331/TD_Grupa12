﻿using Invector.CharacterController;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCharacterState : MonoBehaviour
{
    public HUD Hud;
    public LowHPEffect HpEffect;
    public float HealthRestoreCooldown = 5f;
    public float EnergyRestoreCooldown = 2f;
    public float RegenratePerSecond = 10f;
    public static float SprintingEnergyCostPerSecond = 10f;
    private float timeSinceLastDamage = 0;
    private static float timeSinceLastEnergyaUsage = 0;

    public float MaxHealthPoints = 100.0f;
    private float currentHealthPoints;

    public float MaxEnergyPoints = 500.0f;
    private static bool isAlive = true;

    private HealthBar mHealthBar;
    private EnergyBar mEnergyBar;

    private Animator animator;
    public GameObject SpikeTrap;
    public GameObject SplashTrap;
	
	float slowDownFactor = 1;
	bool isSlowed;
	float duration;
	float startTime;
	float endTime;

    public System.Diagnostics.Stopwatch timer;

    public static float CurrentEnergyPoints { get; private set; }

    void Start()
    {
        timer = new System.Diagnostics.Stopwatch();
        timer.Start();

        animator = GetComponent<Animator>();
        SpikeTrap.transform.localScale = new Vector3(7, 5, 7);
	    HpEffect.InitState(MaxHealthPoints);
        InitHudBars();

        PopupParent.Initialize(gameObject);
	}
	
    void Update()
    {
        Traps();
        HealthPointsRegenerating();
        EnergyPointsRegenerating();
        EnergyManager();
        UpdatePlayerBar();
        RecalculateSlowDownEffect();
    }
 
    private void Traps()
    {
        if (Input.GetKeyDown(KeyCode.Space))
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

        if (Input.GetKeyDown(KeyCode.Alpha2) && ShopController.AvailableSplashTraps != 0 && timer.ElapsedMilliseconds > 1000)
        {
            GameObject tmp = new GameObject();
            tmp.transform.position = new Vector3(transform.position.x, transform.position.y + 0.02f, transform.position.z);
            Instantiate(SplashTrap, tmp.transform);
            ShopController.AvailableSplashTraps--;
        }
		
		HealthBarBillboarding();
    }
	
	private void RecalculateSlowDownEffect() {
		if (isSlowed)
		{
			endTime = Time.time;
			if ((endTime - startTime) >= duration)
			{
				isSlowed = false;
				slowDownFactor = 1;
			}
		}
	}
	
	private void HealthBarBillboarding() 
	{
		Transform healthBar = transform.Find("HealthBar");
		Camera camera = Camera.main;
		Vector3 vector = camera.transform.position - healthBar.transform.position;
		vector.x = vector.z = 0;
		healthBar.transform.LookAt(camera.transform.position - vector);
	}
	
	public void DealCriticlaDamage(DamageParameters damageParameters)
	{
		int randomInt = Random.Range(0, 100);
		Debug.Log("random: " + randomInt + ", criticProbability: " + damageParameters.criticProbability);
		if (randomInt < damageParameters.criticProbability) { damageParameters.damageAmount = damageParameters.damageAmount * 2; Debug.Log("Krytyk!"); }
		DealDamage(damageParameters);
	}

    public void HealthPointsRegenerating()
    {
	    
	    
        if (animator.GetBool("Block"))
        timeSinceLastDamage += Time.deltaTime;

        if (timeSinceLastDamage > HealthRestoreCooldown && currentHealthPoints < MaxHealthPoints)
        {
            currentHealthPoints += RegenratePerSecond * Time.deltaTime;
            if (currentHealthPoints > MaxHealthPoints)
            {
                currentHealthPoints = MaxHealthPoints;
            }
        }
    }

    public void EnergyPointsRegenerating()
    {
        timeSinceLastEnergyaUsage += Time.deltaTime;

        if (timeSinceLastEnergyaUsage > EnergyRestoreCooldown && CurrentEnergyPoints < MaxEnergyPoints)
        {
            CurrentEnergyPoints += RegenratePerSecond * Time.deltaTime;
            if (CurrentEnergyPoints > MaxEnergyPoints) CurrentEnergyPoints = MaxEnergyPoints;
        }
    }

    public void EnergyManager()
    {
        if (animator.GetFloat("InputVertical") > 1.4f && CurrentEnergyPoints > 0)
        {
            timeSinceLastEnergyaUsage = 0f;
            CurrentEnergyPoints -= SprintingEnergyCostPerSecond * Time.deltaTime;
        }
        if (currentHealthPoints <= 0 && !vThirdPersonController.IsTired)
        {
            vThirdPersonController.IsTired = true;
        }
        if (currentHealthPoints < 0) currentHealthPoints = 0;
    }

    public static bool DecreaseEnergy(float input)
    {
        if (CurrentEnergyPoints >= input)
        {
            CurrentEnergyPoints -= input;
            timeSinceLastEnergyaUsage = 0f;
            return true;
        }
        else return false;
    }

    public void DealDamage(DamageParameters damageParameters)
    {
        if (animator.GetCurrentAnimatorStateInfo(1).IsTag("Block"))
        {
            if(DecreaseEnergy(damageParameters.damageAmount))
            {
                animator.SetTrigger("DealDamage");
                damageParameters.damageAmount *= 0.1f;
            }
        }
        currentHealthPoints -= damageParameters.damageAmount;
		timeSinceLastDamage = 0f;
        
        if (currentHealthPoints <= 0 && isAlive) StartCoroutine(Death()); //You died
		if (currentHealthPoints < 0)  currentHealthPoints = 0;

        if (damageParameters.showPopup)
        {
            if (damageParameters.damageSourceObject.CompareTag("Player"))
                PopupParent.CreatePopup(damageParameters.damageAmount.ToString(), transform);
            else
                PopupParent.CreatePopup(damageParameters.damageAmount.ToString(), transform, new Color(255, 100, 100));
        }
        if (!isSlowed || damageParameters.slowDownFactor <= slowDownFactor)
        {
            duration = damageParameters.duration;
            slowDownFactor = damageParameters.slowDownFactor;
            isSlowed = true;
            startTime = Time.time;
        }
    }

    public IEnumerator Death()
    {
        animator.SetTrigger("Death");
        isAlive = false;
        vThirdPersonMotor.lockMovement = true;
        yield return new WaitForSeconds(3f);
        Debug.Log("You died!");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
    }

    private void InitHudBars()
    {
		mHealthBar = Hud.transform.Find("Bars_Panel/HealthBar").GetComponent<HealthBar>();
        mHealthBar.Max = MaxHealthPoints;
        currentHealthPoints = MaxHealthPoints;
        mHealthBar.SetValue(MaxHealthPoints);

        mEnergyBar = Hud.transform.Find("Bars_Panel/EnergyBar").GetComponent<EnergyBar>();
        mEnergyBar.Max = MaxEnergyPoints;
        CurrentEnergyPoints = MaxEnergyPoints;
        mEnergyBar.SetValue(MaxEnergyPoints);
    }

	private void UpdatePlayerBar()
	{
		mHealthBar.SetValue(currentHealthPoints);
		HpEffect.SetCurrentHp(currentHealthPoints);
        mEnergyBar.SetValue(CurrentEnergyPoints);
	}
}
