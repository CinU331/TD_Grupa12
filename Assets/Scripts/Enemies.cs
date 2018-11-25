using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Enemies : MonoBehaviour
{
    private Transform exitGate;
    private int numberOfWaypoint = 0;
    public float speed = 50f;
    public static int resources = 5;
    public static int credits = 10;
    public float iMaxHp = 400;

    public float damage = 20;
    public float iCurrentHp;

    [Header("Mob attack settings")]
    public float attackDistance = 3f;
    public float detourMaxDistance = 20f;
    public float playerMaxDistance = 20f;
    public float detourMaxTimeInSec = 5f;
    public float returnToPathSpeedup = 0.2f;
    private bool isDuringAttackAnimation = false;
    private Vector3 aggroStartingPoint;

    private Animator animator;

    private bool isAlive;
    private bool isAggroed;

    private float timeSinceAggro = 0f;

    float slowDownFactor = 1;
    bool isSlowed = false;
    float duration;
    float startTime;
    float endTime;

    void Start()
    {
        exitGate = Waypoints.waypoints[0];

        animator = GetComponent<Animator>();

        iCurrentHp = iMaxHp;
        isAlive = true;
        isAggroed = false;
    }

    void Update()
    {
        if (!isAlive) {
            return;
        }
        
        RecalculateSlowDownEffect();

        if (!isAggroed) 
        {
            MoveAlongPath();
        } 
        else 
        {
            HandlePlayerAggro();
        }

        HealthBarBillboarding();        
    }

    private void RecalculateSlowDownEffect() {
        if (isSlowed)
        {
            endTime = Time.time;
            if ((endTime - startTime) >= duration)
            {
                isSlowed = false;
                slowDownFactor = 1;
            }
        }
    }

    private void GoToNextWaypoint()
    {
        if (numberOfWaypoint >= Waypoints.waypoints.Length - 1)
        {
            if (GameObject.Find("OrcHB(Clone)"))
            {
                credits -= 20;
            }
            else
                credits -= 10;
            WaveSpawner.aliveEnemies--;
            Destroy(gameObject);
            return;
        }

        numberOfWaypoint++;
        exitGate = Waypoints.waypoints[numberOfWaypoint];
    }

    private void MoveAlongPath() {
        Vector3 directions = exitGate.position - transform.position;
        Move(directions);

        if (Vector3.Distance(transform.position, exitGate.position) <= 0.4f)
        {
            if (numberOfWaypoint == 40)
            {
                if (GameObject.Find("OrcHB(Clone)"))
                {
                    resources -= 4;
                }
                else 
                {
                    resources--;
                }
            }

            if (resources <= 0) 
            {
                LoadEndGameScreen();
            }
            GoToNextWaypoint();
        }
    }

    private void LoadEndGameScreen() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void Move(Vector3 movePoint) 
    {
        RotateTowardsMovementDirection(movePoint);
        transform.Translate(movePoint.normalized * speed * slowDownFactor * Time.deltaTime, Space.World);
    }

    private void RotateTowardsMovementDirection(Vector3 movePoint) 
    {
        Vector3 lookDirection = movePoint.normalized;
        if (lookDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }
    }

    private void HealthBarBillboarding() 
    {
        Transform healthBar = transform.Find("HealthBar");
        Camera camera = Camera.main;
        Vector3 vector = camera.transform.position - healthBar.transform.position;
        vector.x = vector.z = 0;
        healthBar.transform.LookAt(camera.transform.position - vector);
    }

    public void DealDamage(DamageParameters damageParameters)
    {
        if (!isSlowed)
        {
            duration = damageParameters.duration;
            slowDownFactor = damageParameters.slowDownFactor;
            isSlowed = true;
            startTime = Time.time;
        }

        iCurrentHp -= damageParameters.damageAmount;
        transform.Find("HealthBar").Find("Background").Find("Foreground").GetComponent<Image>().fillAmount = iCurrentHp / iMaxHp;
        if (iCurrentHp <= 0 && isAlive)
        {
            isAlive = false;
            animator.SetTrigger("Death");
        }

        GameObject player = GameObject.FindWithTag("Player");
        if (damageParameters.damageSourceObject != null && damageParameters.damageSourceObject == player) 
        {
            StartPlayerAggro();
        }
    }

    private void StartPlayerAggro() 
    {
        if (!isAggroed) 
        {
            aggroStartingPoint = transform.position;
        }

        isAggroed = true;
        timeSinceAggro = 0f;
    }

    private void StopPlayerAggro() 
    {
        animator.SetBool("isAttacking", false);
        isDuringAttackAnimation = false;
        isAggroed = false;
    }

    private void HandlePlayerAggro() 
    {
        timeSinceAggro += Time.deltaTime;

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) {
            float distance = Vector3.Distance(player.transform.position, transform.position);

            if (!isDuringAttackAnimation) 
            {
                if (distance <= attackDistance)
                {
                    RotateTowardsMovementDirection(player.transform.position - transform.position);

                    isDuringAttackAnimation = true;
                    animator.SetBool("isAttacking", true);

                    timeSinceAggro = 0;
                }
                else
                {
                    Move(player.transform.position - transform.position);
                }
            } 
            else if (timeSinceAggro >= detourMaxTimeInSec || 
                     distance >= playerMaxDistance || 
                     Vector3.Distance(transform.position, aggroStartingPoint) >= detourMaxDistance)
            {
                StopPlayerAggro();
            }
            
        }
    }
    
	public void AttackAnimationFinished() 
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) 
        {
            player.SendMessage("DealDamage", new DamageParameters { damageAmount = damage, duration = 0.8f, slowDownFactor = 0.8f, damageSourceObject = gameObject });
        }

        isDuringAttackAnimation = false;
        animator.SetBool("isAttacking", false);
    }

    public void DyingFinished() 
	{
        if (GameObject.Find("OrcHB(Clone)"))
        {
            credits += 87 - numberOfWaypoint;
        }
        else
        {
            credits += (87 - numberOfWaypoint) / 10;
        }
        
        if (WaveSpawner.aliveEnemies > 0)
        {
            WaveSpawner.aliveEnemies--;
        }

        Destroy(gameObject);
	}
}
