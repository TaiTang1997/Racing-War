using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartWpPlayerAR : WeaponsAR
{
    private Vector3 Groundnormal;
    public float lookRadius;
    public float x;
    public float y;
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        MoveWeapons();
    }

    public override void MoveWeapons()
    {
        base.MoveWeapons();
        transform.Translate(x, 0, y);

        //Raycat
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, -PlayerControllerAR.Instance.transform.up, out hit, 10000))
        {
            Groundnormal = hit.normal;
        }

        Quaternion toRotation = Quaternion.FromToRotation(transform.up, Groundnormal) * transform.rotation;
        transform.rotation = toRotation;
        if (EnemyAR.carEnemyChange.gameObject.activeInHierarchy)
        {
            float distance = Vector3.Distance(EnemyAR.carEnemyChange.position, transform.position);// bug k bắn dc enemy ở hành tinh khá       
            if (distance < lookRadius)
            {
                transform.LookAt(EnemyAR.carEnemyChange.position);
            }
        }

        float distance2 = Vector3.Distance(PlayerControllerAR.Instance.transform.position, transform.position);
        if (distance2 > 350f)
        {
            //Effect
            UIManager.Instance.EffectBulletPlayerDestroyAR(gameObject);
            Destroy(gameObject);
        }
    }
}
