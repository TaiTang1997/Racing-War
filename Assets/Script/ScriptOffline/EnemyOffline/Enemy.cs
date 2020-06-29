using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public GameObject Planet;

    public Transform playerTransform;
    NavMeshAgent myNavMesh;

    [SerializeField]
    private float gravity;

    [SerializeField]
    private bool OnGround = false;

    private Rigidbody rb;
    Vector3 Groundnormal;
    public float lookRadius;
    public static bool atk = true;
    [SerializeField]
    private Transform posRayEnemy;
    public static Enemy Instance { get; private set; }
    public float HealEnemy;
    private Vector3 enemyTranformOld;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        myNavMesh = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        playerTransform = GameObject.FindWithTag("Player").transform;
        enemyTranformOld = transform.position;
        atk = true;
    }
    private float x;//0.1 0.2
    private float y;
    [SerializeField]
    private float a;
    [SerializeField]
    private float b;

    public Transform carEnemy;
    public static Transform carEnemyChange;
    private void FixedUpdate()
    {
        if (!ControllerManagerOffline.Instance.gameSates)
            return;

        transform.Translate(x, 0, y);

        // Lấy thông tin trở lại của 1 raycashit - điều kiện có boxcollider
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, -transform.up, out hit, 10))
        {
            Groundnormal = hit.normal;
        }


        Vector3 gravDirection = (transform.position - Planet.transform.position).normalized;
        if (OnGround == false)
        {
            rb.AddForce(gravDirection * -gravity);
        }

        if (playerTransform.gameObject.activeInHierarchy)
        {
            // Tính khoảng cách từ enemy đến player
            float distance = Vector3.Distance(playerTransform.position, transform.position);
            if (distance <= lookRadius)
            {
                carEnemyChange = carEnemy;
                //myNavMesh.transform.LookAt(playerTransform);

                //if (distance <= myNavMesh.stoppingDistance)
                //{
                //    x = 0;
                //    y = 0;
                //}

                if (atk)
                {
                    AttackEnemy();
                    atk = false;
                }

            }
            else
            {
                x = a;
                y = b;
            }
        }


        Quaternion toRotation = Quaternion.FromToRotation(transform.up, Groundnormal) * transform.rotation;
        transform.rotation = toRotation;
    }

    public virtual void AttackEnemy()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("BulletPlayer"))
        {
            var hpEnemy = other.gameObject.GetComponent<Weapons>();
            Destroy(other.gameObject);
            TakeDamageEnemy(hpEnemy.damage);
            UIManager.Instance.EffectBulletPlayerDestroy(other.gameObject);
        }
    }

    public void TakeDamageEnemy(float damage)
    {
        HealEnemy -= damage;
        if (HealEnemy < 0)
        {
            ControllerManagerOffline.Instance.ActiveUIWin();
            UIManager.Instance.EffectDied(gameObject);
            gameObject.SetActive(false);
        }
    }

}

