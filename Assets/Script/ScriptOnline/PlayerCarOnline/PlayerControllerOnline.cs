using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using Photon.Pun;
public class PlayerControllerOnline : MonoBehaviourPun, IPunObservable
{
    public GameObject Planet;
    public Rigidbody rb;
    public Image healthImgFill;
    public Image healthImgFill2;
    public float Health = 100;
    public float MinHeal = 0;
    public float MaxHeal = 100;
    public float speed;
    public float jumpHeight;
    public float gravity;
    public float distanceToGround;
    public float distanceCar;
    public bool OnGround = false;
    public Vector3 Groundnormal;
    public Vector3 gravDirection;
    public Quaternion toRotation;
    public Behaviour[] components;
    public Transform pointAtk;
    public static PlayerControllerOnline Instance { get; private set; }
    public MPManager mp;
    public string username;
    public Text userText;
    public PlayFabAuth playFab;
    private void Awake()
    {
        Instance = this;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(distanceToGround);
            stream.SendNext(Groundnormal);
            stream.SendNext(gravDirection);
            stream.SendNext(toRotation);
            stream.SendNext(OnGround);
            stream.SendNext(Health);
            stream.SendNext(username);
        }
        else if (stream.IsReading)
        {
            distanceToGround = (float)stream.ReceiveNext();
            Groundnormal = (Vector3)stream.ReceiveNext();
            gravDirection = (Vector3)stream.ReceiveNext();
            toRotation = (Quaternion)stream.ReceiveNext();
            OnGround = (bool)stream.ReceiveNext();
            Health = (float)stream.ReceiveNext();
            username = (string)stream.ReceiveNext();
            userText.text = username;
        }
    }

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (photonView.IsMine)
        {
            userText.text = username;
            Planet = GameObject.FindGameObjectWithTag("Planet");
            healthImgFill = GameObject.FindGameObjectWithTag("HealFill").GetComponent<Image>();

            playFab = GameObject.FindGameObjectWithTag("PlayFab").GetComponent<PlayFabAuth>();
            PlayerPlaceholderOnline.Instance.Player = gameObject;
        }
        else
        {
            mp = GameObject.FindGameObjectWithTag("Network").GetComponent<MPManager>();
            //healthImgFill2 = GameObject.FindGameObjectWithTag("HealFill2").GetComponent<Image>();
            healthImgFill2 = mp.healthImgFill2.transform.GetChild(0).GetComponent<Image>();
            for (int i = 0; i < components.Length; i++)
            {
                components[i].enabled = false;
            }

        }

    }

    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            if (!mp.gameSates)
                return;
            if (mp.aPlayerHasWin1001)
                return;
            if (mp.aPlayerHasWin2001)
                return;
            transform.Translate(0, 0, 0.2f);

            if (photonView.ViewID == 1001)
            {
                if (mp.rotaRight)
                {
                    transform.Rotate(0, 150 * Time.deltaTime, 0);
                }
                if (mp.rotaLeft)
                {
                    transform.Rotate(0, -150 * Time.deltaTime, 0);
                }
            }

            if (photonView.ViewID == 2001)
            {
                if (mp.rotaRight)
                {
                    transform.Rotate(0, 150 * Time.deltaTime, 0);
                }
                if (mp.rotaLeft)
                {
                    transform.Rotate(0, -150 * Time.deltaTime, 0);
                }
            }


            if (photonView.ViewID == 1001)//photonView.Owner.NickName == "Player1"
            {
                //Attack
                if (mp.atkPlayer)
                {
                    AttackPlayer();
                    mp.atkPlayer = false;
                }
            }

            if (photonView.ViewID == 2001)//photonView.Owner.NickName == "Player2"
            {
                //Attack
                if (mp.atkPlayer)
                {
                    AttackPlayer2();
                    mp.atkPlayer = false;
                }
            }

            if (mp.jumpPlayer && OnGround)
            {
                rb.AddForce(transform.up * 40000 * jumpHeight * Time.deltaTime);
                mp.jumpPlayer = false;
            }

            // chieu tia mat dat
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(transform.position, -transform.up, out hit, 10))
            {
                distanceToGround = hit.distance;
                Groundnormal = hit.normal;
                if (distanceToGround <= distanceCar)//0.5f
                {
                    OnGround = true;
                }
                else
                {
                    OnGround = false;
                }
            }

            gravDirection = (transform.position - Planet.transform.position).normalized;
            if (OnGround == false)
            {
                rb.AddForce(gravDirection * -gravity);
            }
            toRotation = Quaternion.FromToRotation(transform.up, Groundnormal) * transform.rotation;
            transform.rotation = toRotation;



            healthImgFill.fillAmount = Health / MaxHeal;
            if (Health > MaxHeal)
            {
                Health = MaxHeal;
            }
            if (Health < MinHeal)
            {
                Health = MinHeal;
            }


            // ID:1001-Win  --- ID:2001-Lose
            if (photonView.ViewID == 2001 && Health <= 0)
            {
                mp.SetLose1001();
            }
            if (photonView.ViewID == 1001 && mp.over1001 == true)
            {
                mp.SetWin1001();
            }
            // ID:1001 Lose  --- ID:2001-Win
            if (photonView.ViewID == 1001 && Health <= 0)
            {
                mp.SetLose2001();
            }
            if (photonView.ViewID == 2001 && mp.over2001 == true)
            {
                mp.SetWin2001();
            }

        }
        else
        {
            healthImgFill2.fillAmount = Health / MaxHeal;
            if (Health > MaxHeal)
            {
                Health = MaxHeal;
            }
            if (Health < MinHeal)
            {
                Health = MinHeal;
            }
        }
        //ID Player
        if (photonView.ViewID == 1001)
        {
            gameObject.tag = "PlayerOnline";
        }
        if (photonView.ViewID == 2001)
        {
            gameObject.tag = "PlayerOnline1";
        }



    }

    public virtual void AttackPlayer()
    {

    }
    public virtual void AttackPlayer2()
    {

    }

}
