using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NissanWpEnemyAR : WeaponsEnemyAR
{
    private Vector3 Groundnormal;
    [SerializeField]
    private float lookRadius;
    public float x;
    public float y;

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        MoveWeaponsEnemy();
    }



    public override void MoveWeaponsEnemy()
    {
        base.MoveWeaponsEnemy();
        transform.Translate(x, 0, y);

        if (EnemyAR.carEnemyChange.gameObject.activeInHierarchy)
        {
            //Raycat
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(transform.position, -EnemyAR.carEnemyChange.transform.up, out hit, 10))
            {
                Groundnormal = hit.normal;
            }

            Quaternion toRotation = Quaternion.FromToRotation(transform.up, Groundnormal) * transform.rotation;
            transform.rotation = toRotation;
            float distance2 = Vector3.Distance(EnemyAR.carEnemyChange.position, transform.position);
            if (distance2 > 350f)
            {
                //Effect
                EnemyAR.atk = true;
                UIManager.Instance.EffectBulletEnemyDestroyAR(gameObject);
                Destroy(gameObject);
            }

        }

        float distance = Vector3.Distance(PlayerControllerAR.Instance.transform.position, transform.position);
        if ((distance < lookRadius) && PlayerControllerAR.Instance.meshPlayer.enabled == true)
        {
            transform.LookAt(PlayerControllerAR.Instance.transform.position);
        }


    }
}
