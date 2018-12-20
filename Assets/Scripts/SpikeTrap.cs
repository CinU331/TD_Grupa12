using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class SpikeTrap : AbstractTrap {

    int durubality = 3;
    float cooldown = 5;
    bool isAvailable = false;
    float startTime;
    float endTime;

    List<GameObject> monstersInRange;

    AudioSource audioSource;
    Animation animation;
    Collider collider;

    private SpikeTrap()
    {
        TrapId = 1;
        Name = "Spike trap";
        Cost = 5;
    }

    // Use this for initialization
	void Start () {
	    
        collider = GetComponent<Collider>();
        animation = GetComponent<Animation>();
        audioSource = GetComponent<AudioSource>();

        monstersInRange = new List<GameObject>();
        animation.wrapMode = WrapMode.Once;
    }
	
	// Update is called once per frame
	void Update () {
        if (durubality == 0 && !animation.isPlaying)
        {
            Destroy(gameObject);
        }
        endTime = Time.time;
        float currentDiffrence = endTime - startTime;

        if(currentDiffrence >= cooldown || isAvailable)
        {
            FindMonstersInRnge();
            if(!animation.isPlaying && monstersInRange.Count != 0)
            {
                animation.Play();
                audioSource.Play();
                foreach(GameObject monster in monstersInRange)
                {
                    monster.SendMessage("DealDamage", new DamageParameters { damageAmount = 700, duration = 1.5f, slowDownFactor = 0.3f });
                }
                durubality--;
                isAvailable = false;
                startTime = Time.time;
            }
        }

    }

    void FindMonstersInRnge()
    {
        monstersInRange.Clear();

        float minX = collider.bounds.min.x;
        float maxX = collider.bounds.max.x;
        float minZ = collider.bounds.min.z;
        float maxZ = collider.bounds.max.z;

        GameObject[] allMonsters = GameObject.FindGameObjectsWithTag("Respawn");
        foreach (GameObject monster in allMonsters)
        {
            if (monster.transform.position.x > (minX) && monster.transform.position.x < (maxX) && monster.transform.position.z > (minZ) && monster.transform.position.z < (maxZ))
                monstersInRange.Add(monster);
        }
    }

    public override int TrapId { get; set; }
    public override string Name { get; set; }
    public override int Cost { get; set; }
}
