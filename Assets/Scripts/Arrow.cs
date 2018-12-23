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
            other.gameObject.SendMessage("DealDamage", DamageParameters);
            creator.SendMessage("RemovePair", gameObject);
        }
        else if (other.gameObject.name.Equals("Terrain"))
        {
            creator.SendMessage("RemovePair", gameObject);
        }
    }

}
