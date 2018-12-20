using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class TarSplash : AbstractTrap
{
    private List<GameObject> monsters;
    private int numberOfTargets = 5;

    AudioSource audioSource;

    private TarSplash()
    {
        TrapId = 0;
        Name = "Tar splash trap";
        Cost = 5;
    }
    
    // Use this for initialization
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        monsters = new List<GameObject>();
    }

    // Update is called once per frame
    private void Update()
    {
        AddEnemies();
        CheckAndSlow();
        CleanUp();
    }

    private void AddEnemies()
    {
        GameObject[] allMonsters = GameObject.FindGameObjectsWithTag("Respawn");
        foreach (GameObject monster in allMonsters)
        {
            if (Vector3.Distance(monster.transform.position, transform.position) <= transform.localScale.x / 2)
            {
                if (!monsters.Contains(monster) && monsters.Count < numberOfTargets)
                {
                    monsters.Add(monster);
                    audioSource.Play();
                }
            }
        }
    }

    private int CheckAndSlow()
    {
        int numberOfSlowed = 0;
        for (int i = 0; i < monsters.Count; i++)
        {
            if (monsters[i] != null)
            {
                if (Vector3.Distance(monsters[i].transform.position, transform.position) <= transform.localScale.x / 2)
                {
                    monsters[i].SendMessage("DealDamage", new DamageParameters { damageAmount = 0, duration = 0.1f, slowDownFactor = 0.3f });
                    numberOfSlowed++;
                }
            }
        }

        return numberOfSlowed;
    }

    private void CleanUp()
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            if (monsters[i] != null)
            {
                if (monsters.Count == numberOfTargets && CheckAndSlow() == 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    public override int TrapId { get; set; }
    public override string Name { get; set; }
    public override int Cost { get; set; }
}
