using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
public class TtOnline : PlayerControllerOnline
{
    public GameObject bullet;
    public GameObject bullet2;
    public override void AttackPlayer()
    {
        base.AttackPlayer();
        photonView.RPC("Attack", RpcTarget.All);
    }

    [PunRPC]
    public void Attack()
    {
        GameObject player = GameObject.Instantiate(bullet, pointAtk.position, Quaternion.identity) as GameObject;
    }

    public override void AttackPlayer2()
    {
        base.AttackPlayer2();
        photonView.RPC("Attack2", RpcTarget.All);
    }

    [PunRPC]
    public void Attack2()
    {
        GameObject player = GameObject.Instantiate(bullet2, pointAtk.position, Quaternion.identity) as GameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BulletPlayerOnline")
        {
            if (photonView.ViewID == 2001)
            {
                if (other.gameObject != null)
                {
                    UIManager.Instance.EffectBulletEnemyDestroy(other.gameObject);
                    Destroy(other.gameObject);
                    photonView.RPC("Damage2", RpcTarget.All);
                    if (Health <= 0)
                    {
                        if (photonView.IsMine)
                        {
                            //playFab.SetPlayerLose(1);
                        }

                    }
                }

            }

        }

        if (other.tag == "BulletPlayerOnline1")
        {
            if (photonView.ViewID == 1001)
            {
                if (other.gameObject != null)
                {
                    UIManager.Instance.EffectBulletEnemyDestroy(other.gameObject);
                    Destroy(other.gameObject);
                    photonView.RPC("Damage", RpcTarget.All);
                    if (Health <= 0)
                    {
                        if (photonView.IsMine)
                        {
                            //playFab.SetPlayerLose(1);
                        }
                    }
                }

            }

        }

        if (Planet != null)
        {
            if ((other.transform != Planet.transform) && (!other.tag.Equals("BulletPlayerOnline")) && (!other.tag.Equals("BulletPlayerOnline1")))
            {
                Planet = other.transform.gameObject;

                Vector3 gravDirection = (transform.position - Planet.transform.position).normalized;

                Quaternion toRotation = Quaternion.FromToRotation(transform.up, gravDirection) * transform.rotation;
                transform.rotation = toRotation;

                rb.velocity = Vector3.zero;
                rb.AddForce(gravDirection * gravity);

                PlayerPlaceholderOnline.Instance.NewPlanet(Planet);
            }
        }

    }


    [PunRPC]
    void Damage()
    {
        Health -= 20;

    }

    [PunRPC]
    void Damage2()
    {
        Health -= 20;

    }
}
