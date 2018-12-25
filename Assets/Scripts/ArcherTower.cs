using System.Collections.Generic;
using UnityEngine;

public class ArcherTower : AbstractTower
{
    private float iShootForce = 40f;
    public int iMaxTargets = 3;
    private float iCooldown = 2f;
    private float iSlowDownRatio = 0.8f;
    private DamageParameters damageParameters;

    public GameObject arrowToSpawn;
    public AudioSource audioSource;
    public List<GameObject> inRange;
    private Vector3 startPoint;
    private List<KeyValuePair<GameObject, Vector3>> arrowsAndTargets;
    private float seconds;

    public ParticleSystem explosionEffect;
    public float damageCounter = 0f;
    public float damageLimit = 0;
    public System.Random random;
    
    // Use this for initialization
    private void Start()
    {

        iGameResources = GameObject.Find("GameResources").GetComponent<GameResources>();
        audioSource = GetComponent<AudioSource>();
        inRange = new List<GameObject>();
        arrowsAndTargets = new List<KeyValuePair<GameObject, Vector3>>();

        seconds = Time.time;
        random = new System.Random();
        explosionEffect = transform.GetChild(6).GetComponent<ParticleSystem>();

        iDamage = 150;
        damageParameters = new DamageParameters { damageAmount = iDamage, duration = 0.8f, slowDownFactor = iSlowDownRatio, damageSourceObject = gameObject, showPopup = true };
        iBaseUpgradeCost = 10;
        ColorUtility.TryParseHtmlString("#0000CC", out iUpgradeColor);
        ChangeColor();
        iRange = 20f;
        damageLimit = random.Next(2500, 7500);
    }

    // Update is called once per frame
    private void Update()
    {

        if (Time.time - seconds >= iCooldown)
        {
            FindEnemiesInRange();
            if (inRange.Count != 0)
            {
                seconds = Time.time;
            }
            if (inRange.Count != 0 && arrowsAndTargets.Count < iMaxTargets * 2)
            {
                audioSource.Play();
                for (int i = 0; i < iMaxTargets; i++)
                {
                    int randomTargetIndex = random.Next(inRange.Count);

                    GameObject temporaryArrowObject = Instantiate(arrowToSpawn, transform.position + new Vector3(0f, GetComponent<CapsuleCollider>().bounds.extents.y * 1.25f, 0f), new Quaternion());

                    temporaryArrowObject.GetComponent<CapsuleCollider>().enabled = true;
                    temporaryArrowObject.GetComponent<CapsuleCollider>().isTrigger = true;
                    temporaryArrowObject.AddComponent<Rigidbody>().useGravity = false;
                    temporaryArrowObject.GetComponent<Rigidbody>().isKinematic = false;
                    temporaryArrowObject.GetComponent<Arrow>().DamageParameters = damageParameters;
                    temporaryArrowObject.GetComponent<Arrow>().creator = gameObject;
                    temporaryArrowObject.transform.localScale = new Vector3(2, 2, 2);

                    Vector3 temporaryDestination = new Vector3(inRange[randomTargetIndex].transform.position.x, inRange[randomTargetIndex].transform.position.y + 0.7f, inRange[randomTargetIndex].transform.position.z);

                    temporaryArrowObject.transform.LookAt(inRange[i].transform.position, Vector3.up);
                    temporaryArrowObject.transform.Rotate(80, 0, 0);
                    arrowsAndTargets.Add(new KeyValuePair<GameObject, Vector3>(temporaryArrowObject, temporaryDestination));
                }
            }
           
        }


        for (int i = 0; i < arrowsAndTargets.Count; i++)
        {
            if (arrowsAndTargets[i].Key != null)
            {
                arrowsAndTargets[i].Key.GetComponent<Rigidbody>().velocity = (arrowsAndTargets[i].Value - arrowsAndTargets[i].Key.transform.position).normalized * iShootForce;
            }
        }

    }

    private void FindEnemiesInRange()
    {
        inRange.Clear();
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Respawn");
        foreach (GameObject enemy in objects)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) <= iRange)
            {
                inRange.Add(enemy);
            }
        }

    }

    public void RemovePair(GameObject gameObject)
    {
        GameObject tmp = gameObject;
        arrowsAndTargets.RemoveAll(p => p.Key == gameObject);
        Destroy(tmp);
    }
    public void StopAllAnimations()
    {

    }
    private void OnDestroy()
    {
        if (arrowsAndTargets != null)
        {
            for (int i = 0; i < arrowsAndTargets.Count; i++)
            {
                if (arrowsAndTargets[i].Key != null)
                {
                    GameObject tmp = arrowsAndTargets[i].Key;
                    arrowsAndTargets.RemoveAt(i);
                    GameObject.Destroy(tmp);
                }
                else
                {
                    arrowsAndTargets.RemoveAt(i);
                }
            }
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, iRange);
    }

    public override void UpgradeTower()
    {

        if (IsUpgradeAvailable && iGameResources.Credits >= iBaseUpgradeCost * (iCurrentUpgradeLevel))
        {
            iMaxTargets++;

            StopAllAnimations();

            for (int i = 0; i < arrowsAndTargets.Count; i++)
            {
                Destroy(arrowsAndTargets[i].Key);
            }
            arrowsAndTargets.Clear();

            iDamage = (int)(iDamage * 1.5f);
            iSlowDownRatio -= 0.1f;
            iCooldown -= 0.5f;
            iRange += 2.0f;

            iGameResources.ChangeCreditsCount(-iBaseUpgradeCost * iCurrentUpgradeLevel);
            iCurrentUpgradeLevel++;
            if (iCurrentUpgradeLevel == 2)
            {
                ColorUtility.TryParseHtmlString("#CCCC00", out iUpgradeColor);
            }
            else if (iCurrentUpgradeLevel == 3)
            {
                ColorUtility.TryParseHtmlString("#CC0000", out iUpgradeColor);
            }

            ChangeColor();

            UnityEngine.Debug.Log("Upgraded");
        }
        else
        {
            UnityEngine.Debug.Log("Not able to upgrade");
        }
    }

    public override void ChangeColor()
    {
        transform.Find("Tower_Roof").GetComponent<MeshRenderer>().material.SetColor("_Color", iUpgradeColor);
        transform.Find("Tower_Roof").GetComponent<MeshRenderer>().material.SetColor("_SpecColor", iUpgradeColor);
        transform.Find("Tower_Roof").GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", iUpgradeColor);
        transform.Find("Tower_Roof").GetComponent<MeshRenderer>().material.SetFloat("_Glossiness", 0.01f);
    }
}
