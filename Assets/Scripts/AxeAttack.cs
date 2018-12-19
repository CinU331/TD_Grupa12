using UnityEngine;

public class AxeAttack : MonoBehaviour
{
    public GameObject DamageSourceObject; 
    public float BaseDamage = 200f;
    public float RandomRange = 50f;
    public int CritChance = 15;
    public string EnemyTag = "Respawn";

    private void OnTriggerEnter(Collider other)
    {
        Animator animator = DamageSourceObject.GetComponentInParent<Animator>();
        if (other.CompareTag(EnemyTag) && animator != null &&
            animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            GetComponent<AudioSource>().Play();
            other.gameObject.SendMessage("DealCriticlaDamage",
                new DamageParameters
                {
                    damageAmount = BaseDamage + Random.Range(-RandomRange, RandomRange), duration = 1f,
                    slowDownFactor = 0.6f, criticProbability = CritChance, showPopup = true,
                    damageSourceObject = DamageSourceObject
                });
        }
    }
}
