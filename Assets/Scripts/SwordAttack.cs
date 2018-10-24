using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Respawn" && GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag("Attack")) other.gameObject.SendMessage("DealDamage", 100f);
    }
}
