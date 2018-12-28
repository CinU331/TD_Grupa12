﻿using UnityEngine;
using System.Collections;
using System.Threading;

public class FxSplash : MonoBehaviour {
	private Rigidbody rb;
	public ParticleSystem Ps_Splash;
	public ParticleSystem Ps_Trail;
    public GameObject coveringFire;

	void Start() {
		rb = this.GetComponent<Rigidbody>();
		mh = this.GetComponent<MeshRenderer>();
	}

	void OnCollisionEnter(Collision collision) {
		// Debug-draw all contact points and normals
        if(collision.gameObject.name != "vThirdPersonController")
        {
            GetComponent<AudioSource>().Play();
            mh.enabled = false;
            Ps_Trail.Stop();
            Ps_Splash.Play();
            GameObject [] enemies = GameObject.FindGameObjectsWithTag("Respawn");
            foreach(GameObject enemy in enemies)
            {
                if(Vector3.Distance(enemy.transform.position, transform.position) < 6)
                {
                    enemy.SendMessage("DealDamage", new DamageParameters { damageAmount = 100f, duration = 2f, slowDownFactor = 0.7f, damageSourceObject = GameObject.Find("vThirdPersonController"), showPopup = true });
                    GameObject spawnedFire = Instantiate(coveringFire, enemy.transform);
                    spawnedFire.SendMessage("StartDestruction", 6f);
                    spawnedFire.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                    spawnedFire.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
                    spawnedFire.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
                    GameObject.Find("vThirdPersonController").SendMessage("DoT", enemy);
                }
            }
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            GetComponent<BoxCollider>().enabled = false;
            Invoke("DestroyFx", 2f);
        }
	}

	void DestroyFx()
	{
		Destroy(this.gameObject);
	}


	private MeshRenderer mh;
}
