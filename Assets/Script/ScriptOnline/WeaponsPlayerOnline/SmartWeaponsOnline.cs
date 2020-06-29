using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartWeaponsOnline : WeaponsOnline
{
    public Vector3 Groundnormal;
    public float lookRadius = 10f;
    public Quaternion toRotation;
    public float distance2;
    public GameObject playerEnemy;
    public GameObject player;
    public float distance;


    void Start()
    {
        if (playerEnemy == null)
        {
            playerEnemy = GameObject.FindGameObjectWithTag("PlayerOnline1");
        }
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("PlayerOnline");
        }
    }

    public void FixedUpdate()
    {
        if (player != null)
        {
            transform.Translate(0.1f, 0, 0.2f);
            //Raycat
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(transform.position, -player.transform.up, out hit, 10))
            {
                Groundnormal = hit.normal;
            }

            toRotation = Quaternion.FromToRotation(transform.up, Groundnormal) * transform.rotation;
            transform.rotation = toRotation;


            distance2 = Vector3.Distance(player.transform.position, transform.position);
            if (distance2 > 10f)
            {
                //Effect
                UIManager.Instance.EffectBulletEnemyDestroy(gameObject);
                Destroy(gameObject);
            }
        }



        if (playerEnemy != null)
        {

            distance = Vector3.Distance(playerEnemy.transform.position, transform.position);// bug k bắn dc enemy ở hành tinh khá       
            if (distance < lookRadius)
            {
                transform.LookAt(playerEnemy.transform.position);
            }
        }
    }
}
