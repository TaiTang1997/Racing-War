using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAR : MonoBehaviour
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
    public static EnemyAR Instance { get; private set; }
    public MeshRenderer meshEnemy;
    public float HealEnemy = 100f;
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
        playerTransform = GameObject.FindWithTag("PlayerAR").transform;
        atk = true;
    }
    private float x;
    private float y;
    [SerializeField]
    private float a;
    [SerializeField]
    private float b;

    public Transform carEnemy;
    public static Transform carEnemyChange;
    private void FixedUpdate()
    {
        if (meshEnemy.enabled == true)
        {
            if (!ControllerManager.Instance.gameSates)
                return;
            transform.Translate(x, 0, y);

            // Lấy thông tin trở lại của 1 raycashit - điều kiện có boxcollider
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(transform.position, -transform.up, out hit, 10000))
            {
                Groundnormal = hit.normal;
            }


            Vector3 gravDirection = (transform.position - Planet.transform.position).normalized;
            if (OnGround == false)
            {
                rb.AddForce(gravDirection * -gravity);
            }



            // Tính khoảng cách từ enemy đến player
            if (playerTransform.gameObject.activeInHierarchy)
            {
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
    }

    public virtual void AttackEnemy()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("BulletPlayerAR"))
        {
            var hpEnemy = other.gameObject.GetComponent<WeaponsAR>();
            Destroy(other.gameObject);
            TakeDamageEnemy(hpEnemy.damage);
            UIManager.Instance.EffectBulletPlayerDestroyAR(other.gameObject);
        }
    }

    public void TakeDamageEnemy(float damage)
    {
        HealEnemy -= damage;
        if (HealEnemy < 0)
        {

            ControllerManager.Instance.ActiveUIWinAR();

            UIManager.Instance.EffectDiedAR(gameObject);
            gameObject.SetActive(false);
        }
    }
}
