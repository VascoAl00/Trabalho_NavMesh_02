using UnityEngine;
using UnityEngine.AI;
using Panda;


public class Enemy_Controller: MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private NavMeshAgent agent;

    public float wanderRadius;
    public float wanderTimer;

    private Transform target;
    private float timer;

    GameObject player;

    private float timetoforgetplayer = 15f;

    Vector3 newPos;
    Vector3 playerPos;

    private bool awareOfPlayer;




    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float fireRate = 1f;

    private float lastShotTime;



    public int maxHP = 1;
    private int currentHP = 1;

    [SerializeField] GameObject healthBar;


    private void Start()
    {
        player = GameObject.Find("Player_Bean");
        Enemy_Alarm.goPlayer += receberAviso;

        lastShotTime = -fireRate;
    }
    private void receberAviso(Vector3 receberPos)
    {
        playerPos = receberPos;
        awareOfPlayer = true;
    }


    void Update()
    {


        timetoforgetplayer += Time.deltaTime;

        if (agent.isOnOffMeshLink)
        {

            agent.CompleteOffMeshLink();

        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    [Task]
    public bool SeePlayer()
    {


        RaycastHit hit;
        if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit))
        {

            if (hit.collider.gameObject.CompareTag("Player"))
            {
                timetoforgetplayer = 0;
                awareOfPlayer = true;
                playerPos = player.transform.position;
                return true;

            }


        }

        return false;

    }

    [Task]
    void Move()
    {

        Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
        agent.SetDestination(newPos);

        ThisTask.Succeed();
    }

    [Task]
    bool IsMoving()
    {

        if (agent.velocity == Vector3.zero)
        {
            return false;
        }

        return true;

    }

    [Task]
    public bool ChasePlayer()
    {

        Vector3 newPos = playerPos;
        agent.SetDestination(playerPos);

        return true;

    }

    [Task]
    public bool TimeToForgetPlayer()
    {

        if (timetoforgetplayer < 10)
        {

            timetoforgetplayer++;
            return false;

        }

        return true;
    }

    [Task]
    public bool GetNervous()
    {

        wanderRadius = 0.5f;
        wanderTimer = 0.5f;

        return true;
    }

    [Task]
    public bool GetHappy()
    {

        wanderRadius = 3f;
        wanderTimer = 0.5f;

        return true;
    }

    [Task]
    public bool LastSeenPlayerPos()
    {

        playerPos = player.transform.position;

        return true;
    }

    [Task]
    public bool AwareOfPlayer()
    {

        return awareOfPlayer;

    }

    [Task]
    public bool ForgetPlayer()
    {

        awareOfPlayer = false;

        return true;
    }

    private bool CanShoot()
    {
        return (Time.time - lastShotTime) >= fireRate;
    }


    [Task]
    private void ShootBullet()
    {
           if (CanShoot())
        {

            Vector3 direction = (player.transform.position - transform.position).normalized;

            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
            bulletRigidbody.velocity = direction * bulletSpeed;

            lastShotTime = Time.time;

        }

        ThisTask.Succeed();

    }

    private void TakeDamage()
    {

        currentHP = currentHP - 1;
        healthBar.transform.GetChild(currentHP).gameObject.SetActive(false);

        if (currentHP <= 0)
        {

            Dies();

        }

    }

    private void Dies()
    {

        Destroy(gameObject);

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Bullet"))
        {

            TakeDamage();

        }

    }

}