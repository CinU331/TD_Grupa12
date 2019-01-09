using System.Collections.Generic;
using UnityEngine;

public class MageTower : AbstractTower
{
    public float iCooldown = 0.15f;
    public int iMaxTargets = 3;
    public float iSlowDownFactor = 0.7f;

    public GameObject objectToSpawn;
    public GameObject[] bolts;
    private AudioSource audioSource;
    private List<GameObject> inRange;

    public GameObject specialEffectToSpawn;
    private GameObject spawnedEffect;
    private float damageCounter = 0f;
    private float damageLimit = 0;
    private System.Random random;

    // Use this for initialization
    private void Start()
    {
        iBaseUpgradeCost = 10;
        iDamage = 1;
        iGameResources = GameObject.Find("GameResources").GetComponent<GameResources>();

        ColorUtility.TryParseHtmlString("#000066", out iUpgradeColor);
        ChangeColor();
        random = new System.Random();
        damageLimit = random.Next(2500, 7500);
        audioSource = GetComponent<AudioSource>();

        inRange = new List<GameObject>();
        bolts = new GameObject[iMaxTargets];
        for (int i = 0; i < bolts.Length; i++)
        {
            bolts[i] = Instantiate(objectToSpawn, transform.position, transform.rotation);
            bolts[i].transform.GetChild(0).transform.position = transform.position;
            bolts[i].transform.GetChild(0).transform.position += new Vector3(0, 12, 0);
            bolts[i].SetActive(false);
        }

        InvokeRepeating("FindTarget", 0f, 0.01f);
        InvokeRepeating("AttackEnemy", 0f, iCooldown);
    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void FindTarget()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Respawn");
        foreach (GameObject enemy in objects)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) <= iRange)
            {
                inRange.Add(enemy);
            }
        }
    }

    private void AttackEnemy()
    {
        if (inRange.Count != 0)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }

            if (bolts.Length <= inRange.Count)
            {
                for (int i = 0; i < bolts.Length; i++)
                {
                    if (inRange[i] != null)
                    {
                        bolts[i].transform.GetChild(1).transform.position = inRange[i].transform.position;
                        bolts[i].SetActive(true);
                        inRange[i].SendMessage("DealDamage", new DamageParameters { damageAmount = iDamage, duration = 0.05f, slowDownFactor = iSlowDownFactor, damageSourceObject = gameObject, showPopup = false });
                        inRange[i].SendMessage("PopupForMagicTower", new DamageParameters { damageAmount = iDamage, duration = 0.05f, slowDownFactor = iSlowDownFactor, damageSourceObject = gameObject, showPopup = false });
                        UpdateDamageCounter(inRange[i]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < inRange.Count; i++)
                {
                    if (inRange[i] != null)
                    {
                        bolts[i].transform.GetChild(1).transform.position = inRange[i].transform.position;
                        bolts[i].SetActive(true);
                        inRange[i].SendMessage("DealDamage", new DamageParameters { damageAmount = iDamage, duration = 0.05f, slowDownFactor = iSlowDownFactor, damageSourceObject = gameObject, showPopup = false });
                        inRange[i].SendMessage("PopupForMagicTower", new DamageParameters { damageAmount = iDamage, duration = 0.05f, slowDownFactor = iSlowDownFactor, damageSourceObject = gameObject, showPopup = false });
                        UpdateDamageCounter(inRange[i]);
                    }
                }
                for (int i = inRange.Count; i < bolts.Length; i++)
                {
                    if (inRange[0] != null)
                    {
                        bolts[i].transform.GetChild(1).transform.position = inRange[0].transform.position;
                        inRange[0].SendMessage("DealDamage", new DamageParameters { damageAmount = iDamage, duration = 0.05f, slowDownFactor = iSlowDownFactor, damageSourceObject = gameObject, showPopup = false });
                        inRange[0].SendMessage("PopupForMagicTower", new DamageParameters { damageAmount = iDamage, duration = 0.05f, slowDownFactor = iSlowDownFactor, damageSourceObject = gameObject, showPopup = false });
                        bolts[i].SetActive(true);
                        UpdateDamageCounter(inRange[0]);
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < bolts.Length; i++)
            {
                bolts[i].SetActive(false);
            }
        }
        inRange.Clear();
    }


    public void StopAllAnimations()
    {
        if (bolts != null)
        {
            foreach (GameObject bolt in bolts)
            {
                if (bolt != null)
                {
                    bolt.SetActive(false);
                    GameObject.Destroy(bolt);
                }
            }
            bolts = null;
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, iRange);
    }

    private void OnDestroy()
    {
        StopAllAnimations();
    }


    public override void UpgradeTower()
    {
        if (IsUpgradeAvailable && iGameResources.Credits >= iBaseUpgradeCost * (iCurrentUpgradeLevel))
        {
            iMaxTargets++;
            StopAllAnimations();

            bolts = new GameObject[iMaxTargets];
            for (int i = 0; i < bolts.Length; i++)
            {
                bolts[i] = Instantiate(objectToSpawn, transform.position, transform.rotation);
                bolts[i].transform.GetChild(0).transform.position = transform.position;
                bolts[i].transform.GetChild(0).transform.position += new Vector3(0, 12, 0);
                bolts[i].SetActive(false);
            }

            iDamage = (int)(iDamage * 2f);
            iRange += 2;
            iSlowDownFactor -= 0.1f;
            iGameResources.ChangeCreditsCount(-iBaseUpgradeCost * iCurrentUpgradeLevel);
            iCurrentUpgradeLevel++;

            if (iCurrentUpgradeLevel == 2)
                ColorUtility.TryParseHtmlString("#666600", out iUpgradeColor);
            else if (iCurrentUpgradeLevel == 3)
                ColorUtility.TryParseHtmlString("#660000", out iUpgradeColor);
            ChangeColor();
            BuildingSpot.SpawnRocks();
            Debug.Log("Upgraded");
        }
        else
        {
            Debug.Log("Not able to upgrade");
        }
    }

    public override void ChangeColor()
    {
        transform.Find("Crystal").GetComponent<MeshRenderer>().material.SetColor("_Color", iUpgradeColor);
        transform.Find("Crystal").GetComponent<MeshRenderer>().material.SetColor("_SpecColor", iUpgradeColor);
        transform.Find("Crystal").GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", iUpgradeColor);
        transform.Find("Crystal").GetComponent<MeshRenderer>().material.SetFloat("_Glossiness", 0.01f);

        transform.Find("Banners").GetComponent<MeshRenderer>().material.SetColor("_Color", iUpgradeColor);
        transform.Find("Banners").GetComponent<MeshRenderer>().material.SetColor("_SpecColor", iUpgradeColor);
        transform.Find("Banners").GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", iUpgradeColor);
        transform.Find("Banners").GetComponent<MeshRenderer>().material.SetFloat("_Glossiness", 0.01f);
    }

    public void UpdateDamageCounter(GameObject effectTarget)
    {
        damageCounter += iDamage;
        if (damageCounter >= damageLimit && iCurrentUpgradeLevel == 3)
        {
            spawnedEffect = Instantiate(specialEffectToSpawn, effectTarget.transform);
            spawnedEffect.GetComponent<AudioSource>().Play();
            spawnedEffect.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            spawnedEffect.transform.GetChild(1).GetComponent<ParticleSystem>().Play();

            foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Respawn"))
            {
                if (Vector3.Distance(spawnedEffect.transform.position, gameObject.transform.position) <= spawnedEffect.transform.GetChild(0).transform.localScale.x)
                {
                    gameObject.SendMessage("DealDamage", new DamageParameters { damageAmount = 700f, duration = 2f, slowDownFactor = 0.1f, damageSourceObject = gameObject, showPopup = true });
                }
            }
            damageCounter = 0;
            damageLimit = random.Next(2500, 7500);
        }
    }

}
