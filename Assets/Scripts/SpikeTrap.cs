using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour {

    int durubality = 3;
    float cooldown = 5;
    bool isAvailable = false;
    float startTime;
    float endTime;

    List<GameObject> monstersInRange;
    Animation animation;
    Collider collider;
	// Use this for initialization
	void Start () {
        collider = GetComponent<Collider>();
        animation = GetComponent<Animation>();
        
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
                foreach(GameObject monster in monstersInRange)
                {
                    monster.SendMessage("DealDamage", new DamageParameters { damageAmount = 1000, duration = 1.5f, slowDownFactor = 0.3f });
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

}
