using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IpWeaponsPlayer : Weapons
{
    private Vector3 Groundnormal;
    public float lookRadius;

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        MoveWeapons();
    }

    public override void MoveWeapons()
    {
        base.MoveWeapons();
        transform.Translate(0.15f, 0, 0.25f);

        //Raycat
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, -PlayerController.Instance.transform.up, out hit, 10))
        {
            Groundnormal = hit.normal;
        }

        Quaternion toRotation = Quaternion.FromToRotation(transform.up, Groundnormal) * transform.rotation;
        transform.rotation = toRotation;
        if (Enemy.carEnemyChange.gameObject.activeInHierarchy)
        {
            float distance = Vector3.Distance(Enemy.carEnemyChange.position, transform.position);// bug k bắn dc enemy ở hành tinh khá       
            if (distance < lookRadius)
            {
                transform.LookAt(Enemy.carEnemyChange.position);
            }
        }

        float distance2 = Vector3.Distance(PlayerController.Instance.transform.position, transform.position);
        if (distance2 > 11f)
        {
            //Effect
            UIManager.Instance.EffectBulletPlayerDestroy(gameObject);
            Destroy(gameObject);
        }
    }
}
