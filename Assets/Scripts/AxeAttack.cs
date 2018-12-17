using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeAttack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Respawn" && (GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(1).IsTag("Attack") || GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag("Attack")))
        {
            GetComponent<AudioSource>().Play();
            if (GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(1).IsName("Light Attack"))
                other.gameObject.SendMessage("DealCriticlaDamage", new DamageParameters
                {
                    damageAmount = 150f + Random.Range(-10, 10),
                    duration = 1f,
                    slowDownFactor = 0.6f,
                    criticProbability = 10,
                    showPopup = true,
                    damageSourceObject = GameObject.FindGameObjectWithTag("Player")
                });
            else if (GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("StrongAttack - Running"))
                other.gameObject.SendMessage("DealCriticlaDamage", new DamageParameters
                {
                    damageAmount = 400f,
                    duration = 5f,
                    slowDownFactor = 1f,
                    criticProbability = 10,
                    showPopup = true,
                    damageSourceObject = GameObject.FindGameObjectWithTag("Player")
                });
            else
                other.gameObject.SendMessage("DealCriticlaDamage", new DamageParameters
                {
                    damageAmount = 300f + Random.Range(-20, 20),
                    duration = 1.5f,
                    slowDownFactor = 1f,
                    criticProbability = 10,
                    showPopup = true,
                    damageSourceObject = GameObject.FindGameObjectWithTag("Player")
                });
        }
    }
}
