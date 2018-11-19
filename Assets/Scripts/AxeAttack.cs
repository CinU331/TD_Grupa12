using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeAttack : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Respawn" && GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag("Attack")) other.gameObject.SendMessage("DealDamage", new DamageParameters { damageAmount = 300f, duration = 1f, slowDownFactor = 0.6f });
    }
}
