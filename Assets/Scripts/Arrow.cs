using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {
    public DamageParameters DamageParameters;
    public GameObject creator;
    public int destinyIndex;
	// Use this for initialization
	void Start () {
	  	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Respawn"))
        {
            if (creator.GetComponent<ArcherTower>().iCurrentUpgradeLevel == 3 && creator.GetComponent<ArcherTower>().damageLimit - creator.GetComponent<ArcherTower>().damageCounter <= creator.GetComponent<ArcherTower>().iDamage * 0.5f * creator.GetComponent<ArcherTower>().iMaxTargets)
            {
                creator.transform.GetChild(1).GetComponent<AudioSource>().Play();
            }
            other.GetComponent<Enemies>().DealDamage(DamageParameters);
            creator.GetComponent<ArcherTower>().damageCounter += DamageParameters.damageAmount;
            if (creator.GetComponent<ArcherTower>().damageCounter >= creator.GetComponent<ArcherTower>().damageLimit)
            {
                creator.GetComponent<ArcherTower>().explosionEffect.transform.position = other.transform.position;
                creator.GetComponent<ArcherTower>().explosionEffect.Play();
                if(!creator.transform.GetChild(6).GetComponent<AudioSource>().isPlaying)
                  creator.transform.GetChild(6).GetComponent<AudioSource>().Play();
                creator.GetComponent<ArcherTower>().damageCounter = 0f;
                creator.GetComponent<ArcherTower>().damageLimit = creator.GetComponent<ArcherTower>().random.Next(2500, 7500);

                foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Respawn"))
                {
                    if (Vector3.Distance(transform.position, gameObject.transform.position) <= creator.GetComponent<ArcherTower>().explosionEffect.transform.localScale.x)
                    {
                        gameObject.SendMessage("DealDamage", new DamageParameters { damageAmount = 700f, duration = 2f, slowDownFactor = 0.1f, damageSourceObject = gameObject, showPopup = true });
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
