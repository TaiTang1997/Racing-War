using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TtCar : PlayerController
{
    [SerializeField]
    private Transform pointAtk;
    public GameObject bullet;
    public override void AttackPlayer()
    {
        base.AttackPlayer();
        Attack();
    }

    public void Attack()
    {
        GameObject weaponPlayer = GameObject.Instantiate(bullet, pointAtk.position, Quaternion.identity) as GameObject;
    }
}
