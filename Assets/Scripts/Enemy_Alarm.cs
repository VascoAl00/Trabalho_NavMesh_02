using UnityEngine;
using UnityEngine.AI;
using Panda;


public class Enemy_Alarm : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private NavMeshAgent agent;

    public float wanderRadius;
    public float wanderTimer;

    private Transform target;
    private float timer;

    GameObject player;

    private float timetoforgetplayer = 20f;

    Vector3 newPos;
    Vector3 playerPos;

    private bool awareOfPlayer;

    private Vector3 startingPosition;
    public delegate void homie(Vector3 position);

    public static event homie goPlayer;

    private void Start()
    {
        player = GameObject.Find("Player_Bean");
        startingPosition = transform.position;
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
                goPlayer(playerPos);
                return true;

            }


        }

        return false;

    }

    [Task]
    void Move()
    {
        if (SeePlayer())
        {
            agent.SetDestination(playerPos);
        }
        else
        {
            if (Vector3.Distance(transform.position, agent.destination) < 0.5f)
            {
                // We have reached our destination, set new destination to starting position
                agent.SetDestination(transform.position);
            }
        }

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
        if (timetoforgetplayer < 10 || agent.remainingDistance > 0.1f)
        {
            timetoforgetplayer += Time.deltaTime;
            return false;
        }

        timetoforgetplayer = 0;
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
        agent.SetDestination(startingPosition);

        return true;
    }
}