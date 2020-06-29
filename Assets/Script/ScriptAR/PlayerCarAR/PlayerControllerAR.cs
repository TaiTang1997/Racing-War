using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerAR : MonoBehaviour
{
    public GameObject Planet;
    public GameObject PlayerPlaceholder;
    public static PlayerControllerAR Instance { get; private set; }


    //[SerializeField]
    //private float speed;

    [SerializeField]
    private float jumpHeight;

    [SerializeField]
    private float gravity;

    [SerializeField]
    private bool OnGround = false;

    private Rigidbody rb;

    float distanceToGround;
    Vector3 Groundnormal;
    private bool jumb;
    [SerializeField]
    private Transform posRay;
    [SerializeField]
    private float distanceCar;
    public MeshRenderer meshPlayer;
    private bool rotaRight;
    private bool rotaLeft;
    private bool isJump;
    public bool atk;
    public float x;
    public float z;
    public float HealPlayer;
    public float MinHeal;
    public float MaxHeal;
    public Image healthImgFill;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (meshPlayer.enabled == true)
        {
            if (!ControllerManager.Instance.gameSates)
                return;
            transform.Translate(x, 0, z);

            if (rotaRight)
            {
                transform.Rotate(0, 180 * Time.deltaTime, 0);
            }
            if (rotaLeft)
            {
                transform.Rotate(0, -180 * Time.deltaTime, 0);
            }


            if (isJump && jumb)
            {
                rb.AddForce(transform.up * 40000 * jumpHeight * Time.deltaTime);
                isJump = false;
            }
            //GroundControl
            // Lấy thông tin trở lại của 1 raycashit - điều kiện có boxcollider
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(transform.position, -transform.up, out hit, 30000))
            {

                distanceToGround = hit.distance;
                Groundnormal = hit.normal;
                //Debug.Log(distanceToGround);
                if (distanceToGround <= distanceCar)//0.5f
                {
                    OnGround = true;
                    jumb = true;
                }
                else
                {
                    OnGround = false;
                    jumb = false;
                }
            }

            RaycastHit hit2 = new RaycastHit();
            var forward = transform.TransformDirection(Vector3.forward);
            if (Physics.Raycast(posRay.position, forward, out hit2, 5000f))
            {
                if (hit2.collider.tag == "EnemyAR")
                {
                    //transform.Rotate(Vector3.up * 20);
                    //z = 0;
                }

            }
            else
            {
                z = 4.5f;
            }

            //GRAVITY and ROTATION
            Vector3 gravDirection = (transform.position - Planet.transform.position).normalized;
            if (OnGround == false)
            {
                rb.AddForce(gravDirection * -gravity);
            }
            Quaternion toRotation = Quaternion.FromToRotation(transform.up, Groundnormal) * transform.rotation;
            transform.rotation = toRotation;
        }

    }

    public void RotationRight()
    {
        rotaRight = true;
    }
    public void OffRotationRight()
    {
        rotaRight = false;
    }
    public void RotationLeft()
    {
        rotaLeft = true;
    }
    public void OffRotationLeft()
    {
        rotaLeft = false;
    }


    public void Jump()
    {
        isJump = true;
    }
    public void Attack()
    {
        AttackPlayer();
    }
    //Attack
    public virtual void AttackPlayer()
    {

    }
    //CHANGE PLANET
    private void OnTriggerEnter(Collider collision)
    {
        if ((collision.transform != Planet.transform) && (!collision.tag.Equals("BulletEnemyAR")))
        {
            Debug.Log("NewPlanet");
            Planet = collision.transform.gameObject;

            Vector3 gravDirection = (transform.position - Planet.transform.position).normalized;

            Quaternion toRotation = Quaternion.FromToRotation(transform.up, gravDirection) * transform.rotation;
            transform.rotation = toRotation;

            rb.velocity = Vector3.zero;
            rb.AddForce(gravDirection * gravity);

            PlayerPlaceholder.GetComponent<PlayerHolderAR>().NewPlanet(Planet);

        }

        if (collision.tag.Equals("BulletEnemyAR"))
        {
            var hpPlayer = collision.gameObject.GetComponent<WeaponsEnemyAR>();
            Destroy(collision.gameObject);
            EnemyAR.atk = true;
            TakeDamage(hpPlayer.damage);
            UIManager.Instance.EffectBulletEnemyDestroyAR(collision.gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        HealPlayer -= damage;
        healthImgFill.fillAmount = HealPlayer / MaxHeal;
        if (HealPlayer > MaxHeal)
        {
            HealPlayer = MaxHeal;
        }
        if (HealPlayer < MinHeal)
        {
            HealPlayer = MinHeal;
        }
        if (HealPlayer < 0)
        {
            ControllerManager.Instance.ActiveUILoseAR();
            UIManager.Instance.EffectDiedAR(gameObject);
            gameObject.SetActive(false);
        }
    }

}
