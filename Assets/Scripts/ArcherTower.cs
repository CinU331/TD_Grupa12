using System.Collections.Generic;
using UnityEngine;

public class Tuple<T1, T2>
{
    public T1 First { get; set; }
    public T2 Second { get; set; }
    internal Tuple(T1 first, T2 second)
    {
        First = first;
        Second = second;
    }
}

public class ArcherTower : AbstractTower
{
    private float iArrowSpeed = 50;
    private int iMaxTargets = 3;
    private float iCooldown = 2.4f;
    private float iSlowDownRatio = 0.8f;

    private float iStartTime;
    private float iEndTime;
    public GameObject arrowToSpawn;
    public GameObject[] arrows;
    public AudioSource audioSource;
    private bool isAttackPerformed = false;
    private List<GameObject> inRange;

    private Vector3 startPoint;
    private List<Tuple<int, int>> itemTargets;

    // Use this for initialization
    private void Start()
    {
        iDamage = 80;
        iBaseUpgradeCost = 10;
        ColorUtility.TryParseHtmlString("#0000CC", out iUpgradeColor);
        ChangeColor();
        iGameResources = GameObject.Find("GameResources").GetComponent<GameResources>();
        audioSource = GetComponent<AudioSource>();

        inRange = new List<GameObject>();
        arrows = new GameObject[iMaxTargets];

        for (int i = 0; i < arrows.Length; i++)
        {
            arrows[i] = Instantiate(arrowToSpawn, transform.position + new Vector3(0f, GetComponent<CapsuleCollider>().bounds.extents.y * 1.25f, 0f), Quaternion.Euler(-90f, 0f, i * 5f));
            arrows[i].transform.localScale = new Vector3(2, 2, 2);
        }

        startPoint = arrows[0].transform.position;
        itemTargets = new List<Tuple<int, int>>();
    }

    // Update is called once per frame
    private void Update()
    {
        FindTarget();
        iEndTime = Time.time;
        if (iEndTime - iStartTime >= iCooldown)
        {
            AttackEnemy();
        }

        MoveArrows();
    }

    private void FindTarget()
    {
        if (!isAttackPerformed)
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
    }

    private void AttackEnemy()
    {
        if (!isAttackPerformed)
        {
            if (inRange.Count != 0)
            {
                itemTargets.Clear();
                isAttackPerformed = true;

                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }

                iStartTime = Time.time;
                if (arrows.Length <= inRange.Count)
                {
                    for (int i = 0; i < arrows.Length; i++)
                    {
                        itemTargets.Add(new Tuple<int, int>(i, i));
                    }
                }
                else
                {
                    for (int i = 0; i < inRange.Count; i++)
                    {
                        itemTargets.Add(new Tuple<int, int>(i, i));
                    }
                    for (int i = inRange.Count; i < arrows.Length; i++)
                    {
                        itemTargets.Add(new Tuple<int, int>(i, 0));
                    }
                }
            }
            else
            {
                isAttackPerformed = false;
                return;
            }
        }
    }

    public void MoveArrows()
    {
        if (itemTargets.Count == 0)
        {
            isAttackPerformed = false;
            inRange.Clear();
            return;
        }

        if (isAttackPerformed)
        {

            for (int i = 0; i < itemTargets.Count; i++)
            {
                int indexOfArrow = itemTargets[i].First;
                int indexOfTarget = itemTargets[i].Second;

                if (inRange[indexOfTarget] == null)
                {
                    arrows[indexOfArrow].transform.position = startPoint;
                    itemTargets.RemoveAt(i);
                }
            }


            for (int i = 0; i < itemTargets.Count; i++)
            {
                int indexOfArrow = itemTargets[i].First;
                int indexOfTarget = itemTargets[i].Second;

                if (inRange[indexOfTarget] != null && arrows[indexOfArrow] != null)
                {
                    if (Vector3.Distance(inRange[indexOfTarget].transform.position, arrows[indexOfArrow].transform.position) < 0.5)
                    {
                        inRange[indexOfTarget].SendMessage("DealDamage", new DamageParameters { damageAmount = iDamage, duration = 0.8f, slowDownFactor = iSlowDownRatio, damageSourceObject = gameObject });//damage);
                        arrows[indexOfArrow].transform.position = startPoint;
                        itemTargets.RemoveAt(i);
                    }
                    else
                    {
                        Vector3 direction = inRange[indexOfTarget].transform.position - arrows[indexOfArrow].transform.position;
                        float distance = iArrowSpeed * Time.deltaTime;
                        arrows[indexOfArrow].transform.Translate(direction.normalized * distance, Space.World);

                    }
                }
            }
        }
    }

    public void StopAllAnimations()
    {

    }
    private void OnDestroy()
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            GameObject.Destroy(arrows[i]);
        }
        arrows = null;
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
            arrows = new GameObject[iMaxTargets];

            for (int i = 0; i < arrows.Length; i++)
            {
                arrows[i] = Instantiate(arrowToSpawn, transform.position + new Vector3(0f, GetComponent<CapsuleCollider>().bounds.extents.y * 1.25f, 0f), Quaternion.Euler(-90f, 0f, i * 5f));
                arrows[i].transform.localScale = new Vector3(2, 2, 2);
            }
            startPoint = arrows[0].transform.position;

            iDamage = (int)(iDamage * 1.5f);
            iSlowDownRatio -= 0.1f;
            iCooldown -= 0.5f;

            iGameResources.ChangeCreditsCount(-iBaseUpgradeCost * iCurrentUpgradeLevel);
            iCurrentUpgradeLevel++;
            if (iCurrentUpgradeLevel == 2)
                ColorUtility.TryParseHtmlString("#CCCC00", out iUpgradeColor);
            else if (iCurrentUpgradeLevel == 3)
                ColorUtility.TryParseHtmlString("#CC0000", out iUpgradeColor);
            ChangeColor();

            Debug.Log("Upgraded");
        }
        else
        {
            Debug.Log("Not able to upgrade");
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
