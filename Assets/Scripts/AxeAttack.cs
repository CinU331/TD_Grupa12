using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeAttack : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Respawn" && GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            other.gameObject.SendMessage("DealCriticlaDamage", new DamageParameters { damageAmount = 300f + Random.Range(-5.0f, 5.0f), duration = 1f, slowDownFactor = 0.6f, criticProbability = 25, showPopup=true, damageSourceObject = GameObject.FindWithTag("Player") });
    }
}
