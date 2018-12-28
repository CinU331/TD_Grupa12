using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public DamageParameters DamageParameters;
    public GameObject creator;
    public int destinyIndex;
    public GameObject coveringFire;
    private GameObject spawnedFire;

    // Use this for initialization
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Respawn"))
        {
            if (creator.GetComponent<ArcherTower>().iCurrentUpgradeLevel == 3 && creator.GetComponent<ArcherTower>().damageLimit - creator.GetComponent<ArcherTower>().damageCounter <= creator.GetComponent<ArcherTower>().iDamage * 0.5f * creator.GetComponent<ArcherTower>().iMaxTargets)
            {
                if (Time.time - creator.GetComponent<ArcherTower>().lastIgnition >= 4)
                {
                    creator.transform.GetChild(1).GetComponent<AudioSource>().Play();
                    creator.GetComponent<ArcherTower>().lastIgnition = Time.time;
                }
            }
            other.GetComponent<Enemies>().DealDamage(DamageParameters);
            creator.GetComponent<ArcherTower>().damageCounter += DamageParameters.damageAmount;

            if (creator.GetComponent<ArcherTower>().damageCounter >= creator.GetComponent<ArcherTower>().damageLimit)
            {
                creator.GetComponent<ArcherTower>().explosionEffect.transform.position = other.transform.position;
                creator.GetComponent<ArcherTower>().explosionEffect.Play();
                creator.transform.GetChild(6).GetComponent<AudioSource>().Play();
                creator.GetComponent<ArcherTower>().damageCounter = 0f;
                creator.GetComponent<ArcherTower>().damageLimit = creator.GetComponent<ArcherTower>().random.Next(2500, 7500);

                foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Respawn"))
                {
                    float dst = Vector3.Distance(transform.position, enemy.transform.position);
                    if ( dst <= creator.GetComponent<ArcherTower>().explosionEffect.transform.localScale.x)
                    {
                        enemy.SendMessage("DealDamage", new DamageParameters { damageAmount = 200f, duration = 2f, slowDownFactor = 0.4f, damageSourceObject = enemy, showPopup = true });

                        Vector3 dir = enemy.transform.position - transform.position;
                        StartCoroutine(CanonTower.MoveOverSeconds(enemy, new Vector3(enemy.transform.position.x + 2 * dir.x / dst, enemy.transform.position.y, enemy
                            .transform.position.z + 2 * dir.z / dst), 0.3f));

                        GameObject spawnedFire = Instantiate(coveringFire, enemy.transform);
                        spawnedFire.SendMessage("StartDestruction", 6f);
                        spawnedFire.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                        spawnedFire.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
                        spawnedFire.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
                        GameObject.Find("vThirdPersonController").SendMessage("DoT", enemy);

                    }
                }
            }

            creator.GetComponent<ArcherTower>().RemovePair(gameObject);
        }
        else if (other.gameObject.name.Equals("Terrain"))
        {
            creator.SendMessage("RemovePair", gameObject);
        }
    }
}
