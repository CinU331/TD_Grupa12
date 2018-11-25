using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterState : MonoBehaviour {

	public float healthPoints = 100.0f;
	private bool isAlive = true;

	private Animator animator;

	void Start () {
		animator = GetComponent<Animator>();
	}
	
	void Update () {
		
	}

	public void DealDamage(DamageParameters damageParameters)
    {
        healthPoints -= damageParameters.damageAmount;
        
        if (healthPoints <= 0 && isAlive)
        {
			animator.SetTrigger("Death");
			isAlive = false;
			Debug.Log("You died!");
        }

		UpdatePlayerHealthBar();
    }

	public void DyingFinished() 
	{
		Debug.Log("Dying animation finished!");
	}

	private void UpdatePlayerHealthBar()
	{

	}
}
