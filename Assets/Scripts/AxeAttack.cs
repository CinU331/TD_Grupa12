using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeAttack : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Respawn" && GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            GetComponent<AudioSource>().Play();
            other.gameObject.SendMessage("DealCriticlaDamage", new DamageParameters { damageAmount = 200f + Random.Range(-50.0f, 50.0f), duration = 1f, slowDownFactor = 0.6f, criticProbability = 15, showPopup = true, damageSourceObject = GameObject.FindWithTag("Player") });

        }
    }
}
