using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TtEnemyAR : EnemyAR
{
    [SerializeField]
    private Transform pointAtkEnemy;
    public GameObject bullet;
    public override void AttackEnemy()
    {
        base.AttackEnemy();
        Attack();
    }

    public void Attack()
    {
        GameObject weaponEnemy = GameObject.Instantiate(bullet, pointAtkEnemy.position, Quaternion.identity) as GameObject;
    }
}
