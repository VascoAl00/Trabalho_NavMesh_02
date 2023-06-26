    using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class COMPLETE_PlayerController : MonoBehaviour
{
    [SerializeField]private Camera cam;
    [SerializeField]private NavMeshAgent agent;




    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float shootingRange = 10f;

    public float fireRate = 0.5f;
    private float lastShotTime;

    public int maxHP = 3;
    private int currentHP = 3;

    [SerializeField] GameObject healthBar;


    private void Start()
    {
        lastShotTime = -fireRate;
    }



    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }

        if (agent.isOnOffMeshLink)
        {

            agent.CompleteOffMeshLink();

        }

        if (Input.GetKeyDown(KeyCode.E) && CanShoot())
        {
            ShootAtClosestEnemy();
        }
    }

    private bool CanShoot()
    {
        return (Time.time - lastShotTime) >= fireRate;
    }

    private void ShootAtClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0)
        {
            return; // No enemies found
        }

        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance < closestDistance && distance <= shootingRange)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            Vector3 direction = closestEnemy.transform.position - transform.position;
            direction.Normalize();

            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
            bulletRigidbody.velocity = direction * bulletSpeed;

            lastShotTime = Time.time;
        }
    }

    private void TakeDamage()
    {

        currentHP = currentHP - 1;
        healthBar.transform.GetChild(currentHP).gameObject.SetActive(false);

        if (currentHP <= 0)
        {

            GameOver();

        }

    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("BulletEnemy"))
        {

            TakeDamage();

        }

        if (other.CompareTag("Victory"))
        {

            GameVictory();

        }
    }

    private void GameOver()
    {

        SceneManager.LoadScene(1);

    }

    private void GameVictory()
    {

        SceneManager.LoadScene(2);

    }
}