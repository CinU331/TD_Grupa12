using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	void Start () {
		animator = GetComponent<Animator>();

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
	}

	public void DealDamage(DamageParameters damageParameters)
    {
        currentHealthPoints -= damageParameters.damageAmount;

		timeSinceLastDamage = 0f;
        
        if (currentHealthPoints <= 0 && isAlive)
        {
			animator.SetTrigger("Death");
			isAlive = false;
			Debug.Log("You died!");
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

	public void DyingFinished() 
	{
		Debug.Log("Dying animation finished!");
	}

	private void UpdatePlayerHealthBar()
	{
		mHealthBar.SetValue(currentHealthPoints);
	}
}
