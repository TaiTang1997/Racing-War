using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject Planet;
    public GameObject PlayerPlaceholder;
    public static PlayerController Instance { get; private set; }


    [SerializeField]
    private float speed;

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
    private bool rotaRight;
    private bool rotaLeft;
    private bool isJump;
    public float HealPlayer;
    public float MinHeal;
    public float MaxHeal;
    public Image healthImgFill;
    private Vector3 playerTranformOld;
    public float x;
    public float z;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        playerTranformOld = transform.position;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!ControllerManagerOffline.Instance.gameSates)
            return;

        transform.Translate(x, 0, z);

        //Local Rotation

        if (rotaRight)
        {
            transform.Rotate(0, 120 * Time.deltaTime, 0);
        }
        if (rotaLeft)
        {
            transform.Rotate(0, -120 * Time.deltaTime, 0);
        }

        //Jump
        if (isJump && jumb)
        {
            rb.AddForce(transform.up * 40000 * jumpHeight * Time.deltaTime);
            isJump = false;
        }


        //GroundControl
        // Lấy thông tin trở lại của 1 raycashit - điều kiện có boxcollider
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, -transform.up, out hit, 10))
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
        if (Physics.Raycast(posRay.position, forward, out hit2, 3f))
        {
            if (hit2.collider.tag == "Enemy")
            {
                transform.Rotate(Vector3.up * 20);
            }
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

    public void OnRotationRight()
    {
        rotaRight = true;
    }
    public void OffRotationRight()
    {
        rotaRight = false;
    }
    public void OnRotationLeft()
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
    public void AtkPlayer()
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
        if ((collision.transform != Planet.transform) && (!collision.tag.Equals("BulletEnemy")))
        {
            Planet = collision.transform.gameObject;

            Vector3 gravDirection = (transform.position - Planet.transform.position).normalized;

            Quaternion toRotation = Quaternion.FromToRotation(transform.up, gravDirection) * transform.rotation;
            transform.rotation = toRotation;

            rb.velocity = Vector3.zero;
            rb.AddForce(gravDirection * gravity);

            PlayerPlaceholder.GetComponent<PlayerPlaceholder>().NewPlanet(Planet);

        }

        if (collision.tag.Equals("BulletEnemy"))
        {
            var hpPlayer = collision.gameObject.GetComponent<WeaponsEnemy>();
            Destroy(collision.gameObject);
            Enemy.atk = true;
            TakeDamage(hpPlayer.damage);
            UIManager.Instance.EffectBulletEnemyDestroy(collision.gameObject);
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
        if (HealPlayer <= 0)
        {
            ControllerManagerOffline.Instance.ActiveUILose();
            UIManager.Instance.EffectDied(gameObject);
            gameObject.SetActive(false);
        }
    }

}
