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

public class ArcherTower : MonoBehaviour
{
    public float range;
    public float arrowSpeed = 20;
    public int maxTargets = 1;
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
        audioSource = GetComponent<AudioSource>();

        inRange = new List<GameObject>();
        arrows = new GameObject[maxTargets];

        for (int i = 0; i < arrows.Length; i++)
        {
            arrows[i] = Instantiate(arrowToSpawn, transform.position + new Vector3(0f, GetComponent<CapsuleCollider>().bounds.extents.y * 1.25f, 0f), Quaternion.Euler(-90f, 0f, i * 5f));
            arrows[i].transform.localScale = new Vector3(3, 3, 3);
        }

        startPoint = arrows[0].transform.position;
        itemTargets = new List<Tuple<int, int>>();
    }

    // Update is called once per frame
    private void Update()
    {
        FindTarget();
        AttackEnemy();
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
                if (Vector3.Distance(transform.position, enemy.transform.position) <= range)
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
                        inRange[indexOfTarget].SendMessage("DealDamage", 100f);
                        arrows[indexOfArrow].transform.position = startPoint;
                        itemTargets.RemoveAt(i);
                    }
                    else
                    {
                        Vector3 direction = inRange[indexOfTarget].transform.position - arrows[indexOfArrow].transform.position;
                        float distance = arrowSpeed * Time.deltaTime;
                //        arrows[indexOfArrow].transform.rotation = Quaternion.Euler(-arrows[indexOfArrow].transform.rotation.eulerAngles);
                        arrows[indexOfArrow].transform.Translate(direction.normalized * distance, Space.World);
                 //       arrows[indexOfArrow].transform.rotation = Quaternion.Euler(-arrows[indexOfArrow].transform.rotation.eulerAngles);

                    }
                }
            }
        }
    }
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
