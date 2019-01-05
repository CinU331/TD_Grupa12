using UnityEngine;

public class AxeAttack : MonoBehaviour
{
    public GameObject DamageSourceObject; 
    public float BaseDamage = 200f;
    public float RandomRange = 50f;
    public int CritChance = 15;
    public string EnemyTag = "Respawn";

    private void OnTriggerEnter(Collider collider)
    {
        Animator animator = DamageSourceObject.GetComponentInParent<Animator>();
        if (collider.CompareTag(EnemyTag) && (animator.GetCurrentAnimatorStateInfo(1).IsTag("Attack") || animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")))        {
            GetComponent<AudioSource>().Play();
            if (animator.GetCurrentAnimatorStateInfo(1).IsName("Light Attack"))
                collider.gameObject.SendMessage("DealCriticlaDamage", new DamageParameters
                {
                    damageAmount = 150f + Random.Range(-10, 10),
                    duration = 1f,
                    slowDownFactor = 0.6f,
                    criticProbability = 10,
                    showPopup = true,
                    damageSourceObject = GameObject.FindGameObjectWithTag("Player")
                });
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("StrongAttack - Running")) Debug.Log("Special Attack");
            else
                collider.gameObject.SendMessage("DealCriticlaDamage", new DamageParameters
                {
                    damageAmount = 300f + Random.Range(-20, 20),
                    duration = 2f,
                    slowDownFactor = 0.4f,
                    criticProbability = 10,
                    showPopup = true,
                    damageSourceObject = GameObject.FindGameObjectWithTag("Player")
                });
        }
    }
}
