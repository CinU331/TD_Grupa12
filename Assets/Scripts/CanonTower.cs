﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonTower : AbstractTower
{
    private bool isShotInProgress = false;
    private float iSplashRange = 5;
    private float iTurnSpeed = 10;
    private float iBallSpeed = 25;
    private float iCooldown = 6f;
    private float iSlowDownRatio = 0.3f;
    private ParticleSystem directionalSmoke;
    private ParticleSystem smallExplosion;

    private Transform cannonBallTransform;
    private List<GameObject> inRange;

    private GameObject mockBall;
    public GameObject cannonBall;
    public GameObject shockWave;
    private GameObject target;

    // Use this for initialization
    private void Start()
    {

        iDamage = 500;
        iBaseUpgradeCost = 15;

        ColorUtility.TryParseHtmlString("#000066", out iUpgradeColor);
        ChangeColor();
        iGameResources = GameObject.Find("GameResources").GetComponent<GameResources>();

        mockBall = new GameObject();
        mockBall.transform.position = cannonBall.transform.position;
        mockBall.transform.parent = transform.GetChild(1);
        cannonBallTransform = transform.GetChild(1).GetChild(2);

        inRange = new List<GameObject>();
        InvokeRepeating("FindTarget", 0f, 0.05f);
        InvokeRepeating("UpdateRotation", 0f, 0.05f);
        InvokeRepeating("AttackEnemy", 0f, iCooldown);
        
        directionalSmoke = transform.GetChild(2).GetChild(0).GetComponent<ParticleSystem>();
        ParticleSystem.MainModule mM = directionalSmoke.main;
        mM.startSize = 5;
        
        smallExplosion = transform.GetChild(3).GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    private void Update()
    {

        if (target == null)
        {
            cannonBall.transform.position = mockBall.transform.position;
            isShotInProgress = false;
            return;
        }

        if (isShotInProgress && target != null)
        {
            smallExplosion.transform.position = mockBall.transform.position;
            smallExplosion.Play();

            if (Vector3.Distance(cannonBall.transform.position, target.transform.position) < 1)
            {
                directionalSmoke.transform.position = target.transform.position;
                directionalSmoke.Play();

                if (iCurrentUpgradeLevel == 3)
                {
                    shockWave.transform.position = target.transform.position;
                    shockWave.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                }

                target.SendMessage("DealDamage", new DamageParameters { damageAmount = iDamage, duration = 2.000f, slowDownFactor = iSlowDownRatio, damageSourceObject = gameObject, showPopup = true });
                if(iCurrentUpgradeLevel == 3)
                {
                    Vector3 dir = target.transform.position - cannonBall.transform.position;
                    StartCoroutine(MoveOverSeconds(target, new Vector3(target.transform.position.x + dir.x * 5, target.transform.position.y, target.transform.position.z + dir.z * 5), 0.3f));
                }
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Respawn");

                float splashDamage = iDamage * 0.33f;
                for (int i = 0; i < enemies.Length; i++)
                {
                    float dst = Vector3.Distance(enemies[i].transform.position, target.transform.position);
                    if (dst < iSplashRange && enemies[i] != target)
                    {
                        enemies[i].SendMessage("DealDamage", new DamageParameters { damageAmount = splashDamage, duration = 1.200f, slowDownFactor = iSlowDownRatio * 1.5f, damageSourceObject = gameObject, showPopup = true });
                        if (iCurrentUpgradeLevel == 3)
                        {
                            Vector3 dir = enemies[i].transform.position - cannonBall.transform.position;
                            StartCoroutine(MoveOverSeconds(enemies[i], new Vector3(enemies[i].transform.position.x + 2 * dir.x / dst, enemies[i].transform.position.y, enemies[i].transform.position.z + 2 * dir.z / dst), 0.3f));

                        }
                    }
                }

                cannonBall.transform.position = mockBall.transform.position;


                inRange.Clear();
                isShotInProgress = false;
                target = null;
                return;
            }

            Vector3 direction = target.transform.position - cannonBallTransform.position;
            float distance = iBallSpeed * Time.deltaTime;
            cannonBallTransform.Translate(direction.normalized * distance, Space.World);
        }
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

        if (inRange.Count != 0)
        {
            target = inRange[0];
        }
        else
        {
            target = null;
        }
        inRange.Clear();
    }

    private void UpdateRotation()
    {
        if (target == null)
        {
            return;
        }

        transform.GetChild(1);
        Vector3 direction = target.transform.position - transform.GetChild(1).transform.position;
        Quaternion lookQuater = Quaternion.LookRotation(direction);

        Vector3 newRotation = Quaternion.Lerp(transform.GetChild(1).rotation, lookQuater, Time.deltaTime * iTurnSpeed).eulerAngles;
        transform.GetChild(1).rotation = Quaternion.Euler(0f, newRotation.y, 0f);
    }

    private void AttackEnemy()
    {

        if (target != null)
        {
            isShotInProgress = true;
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.Stop();
            audioSource.Play();
        }
        inRange.Clear();
    }

    
    public void StopAllAnimations()
    {

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

            CancelInvoke("AttackEnemy");
            iCooldown *= 0.9f;
            InvokeRepeating("AttackEnemy", 0f, iCooldown);

            iSplashRange *= 1.30f;
            iDamage = (int)(1.30f * iDamage);
            iRange += 3.0f;

            iGameResources.ChangeCreditsCount(-iBaseUpgradeCost * iCurrentUpgradeLevel);
            iCurrentUpgradeLevel++;
            if (iCurrentUpgradeLevel == 2)
                ColorUtility.TryParseHtmlString("#666600", out iUpgradeColor);
            else if (iCurrentUpgradeLevel == 3)
                ColorUtility.TryParseHtmlString("#660000", out iUpgradeColor);
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
        transform.FindDeepChild("Tower_Base_Deco").GetComponent<MeshRenderer>().material.SetColor("_Color", iUpgradeColor);
        transform.FindDeepChild("Tower_Base_Deco").GetComponent<MeshRenderer>().material.SetColor("_SpecColor", iUpgradeColor);
        transform.FindDeepChild("Tower_Base_Deco").GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", iUpgradeColor);
        transform.FindDeepChild("Tower_Base_Deco").GetComponent<MeshRenderer>().material.SetFloat("_Glossiness", 0.01f);

        transform.FindDeepChild("Tower_Top_Deco").GetComponent<MeshRenderer>().material.SetColor("_Color", iUpgradeColor);
        transform.FindDeepChild("Tower_Top_Deco").GetComponent<MeshRenderer>().material.SetColor("_SpecColor", iUpgradeColor);
        transform.FindDeepChild("Tower_Top_Deco").GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", iUpgradeColor);
        transform.FindDeepChild("Tower_Top_Deco").GetComponent<MeshRenderer>().material.SetFloat("_Glossiness", 0.01f);
    }


    public static IEnumerator MoveOverSpeed(GameObject objectToMove, Vector3 end, float speed)
    {
        // speed should be 1 unit per second
        while (objectToMove.transform.position != end)
        {
            objectToMove.transform.position = Vector3.MoveTowards(objectToMove.transform.position, end, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
    public static IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 end, float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        while (elapsedTime < seconds)
        {
            objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = end;
    }
}
