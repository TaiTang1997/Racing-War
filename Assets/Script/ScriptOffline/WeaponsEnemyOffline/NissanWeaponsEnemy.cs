using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NissanWeaponsEnemy : WeaponsEnemy
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

        if (Enemy.carEnemyChange.gameObject.activeInHierarchy)
        {
            //Raycat
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(transform.position, -Enemy.carEnemyChange.transform.up, out hit, 10))
            {
                Groundnormal = hit.normal;
            }

            Quaternion toRotation = Quaternion.FromToRotation(transform.up, Groundnormal) * transform.rotation;
            transform.rotation = toRotation;

            float distance2 = Vector3.Distance(Enemy.carEnemyChange.position, transform.position);
            if (distance2 > 11f)
            {
                //Effect
                Enemy.atk = true;
                UIManager.Instance.EffectBulletEnemyDestroy(gameObject);
                Destroy(gameObject);
            }
        }

        float distance = Vector3.Distance(PlayerController.Instance.transform.position, transform.position);
        if (distance < lookRadius)
        {
            transform.LookAt(PlayerController.Instance.transform.position);
        }
    }
}
