using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    [SerializeField]
    private Transform shootingTargetTransform;
    [SerializeField]
    private Transform movementTargetTransform;

    [SerializeField]
    private float speed = 1f;


    [Header("Weapon")]
    [SerializeField]
    private float fireRate = 1f;
    [SerializeField]
    private float damage = 1f;
    [SerializeField]
    private float accuracy = 1f;
    [SerializeField]
    private float preferredRange = 5f;

    [SerializeField]
    private BulletController bullet;

    private enum EnemyMode
    {
        SPAWNING,
        POSITIONING,
        ATTACKING,
        DYING
    }
    private EnemyMode mode = EnemyMode.SPAWNING;

	// Use this for initialization
	void Start () {
        WalkTowardsTarget();
	}
	
	// Update is called once per frame
	void Update () {
        switch (mode)
        {
            case EnemyMode.SPAWNING:
                Spawn();
                break;
            case EnemyMode.POSITIONING:
                Position();
                break;
            case EnemyMode.ATTACKING:
                Attack();
                break;
            case EnemyMode.DYING:
                Die();
                break;
        }
    }

    //##########################################################################################//
    // Spawning methods                                                                         //
    //##########################################################################################//

    private void Spawn()
    {
        SetupPosition();
    }

    //##########################################################################################//
    // Positioning methods                                                                      //
    //##########################################################################################//

    private void SetupPosition()
    {
        transform.LookAt(shootingTargetTransform);

        movementTargetTransform.position = shootingTargetTransform.position + preferredRange * (Quaternion.AngleAxis(Random.Range(-15, 15), shootingTargetTransform.up) * -transform.forward);

        mode = EnemyMode.POSITIONING;
    }

    private void Position()
    {
        WalkTowardsTarget();

        if (Vector3.Distance(transform.position, movementTargetTransform.position) < speed * Time.deltaTime)
        {
            SetupAttack();
        }
    }

    private void WalkTowardsTarget()
    {
        transform.LookAt(movementTargetTransform, shootingTargetTransform.up);

        if (Vector3.Distance(transform.position, movementTargetTransform.position) < speed * Time.deltaTime)
        {
            transform.position = movementTargetTransform.position;
        }
        else
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    //##########################################################################################//
    // Attacking methods                                                                        //
    //##########################################################################################//

    private void SetupAttack()
    {
        mode = EnemyMode.ATTACKING;
        Invoke("Shoot", 2f);
    }

    private void Attack()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(shootingTargetTransform.transform.position - transform.position), Time.deltaTime);
    }

    private void Shoot()
    {
        bullet.ShootRay(damage, accuracy);

        Invoke("Shoot", 2f);

        Debug.Log("bang");
    }

    //##########################################################################################//
    // Dying methods                                                                            //
    //##########################################################################################//

    private void Die()
    {

    }
}
