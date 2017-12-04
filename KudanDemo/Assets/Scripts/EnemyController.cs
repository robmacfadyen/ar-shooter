﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    //[SerializeField]
    private Transform shootingTargetTransform;
    [SerializeField]
    private GameObject movementTargetPrefab;
    //[SerializeField]
    private Transform movementTargetTransform;

    [SerializeField]
    private float speed = 1f;

    [SerializeField]
    private float maxHealth = 100f;
    private float health = 0;


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

    [SerializeField]
    private MeshRenderer mesh;

    private GameContent game;

    private enum EnemyMode
    {
        SPAWNING,
        POSITIONING,
        ATTACKING,
        DYING
    }
    private EnemyMode mode = EnemyMode.SPAWNING;

	// Use this for initialization
    //void Start () {
    //    Spawn();
    //}

    public void Init()
    {
        mode = EnemyMode.SPAWNING;

        health = maxHealth;

        game = GetComponentInParent<GameContent>();
    }

    void Awake()
    {
        movementTargetTransform = Instantiate(movementTargetPrefab).transform;

        shootingTargetTransform = GameObject.FindGameObjectWithTag("Objective").transform;
    }

    public void MakeTarget()
    {

    }

    void OnEnable()
    {
        if (mode == EnemyMode.ATTACKING)
        {
            Invoke("Shoot", Random.Range(0, fireRate));
        }
    }

    void OnDisable()
    {
        CancelInvoke();
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
    // Damage methods                                                                           //
    //##########################################################################################//

    public void Hit(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }

        mesh.material.color = Color.Lerp(Color.red, Color.white, health / maxHealth);

        Debug.Log(health);
    }

    public bool IsDead()
    {
        return (health <= 0);
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
        movementTargetTransform.parent = transform.parent;
        Debug.Log(transform.parent);

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
        Invoke("Shoot", fireRate);
    }

    private void Attack()
    {
        Quaternion currentRotation = transform.rotation;
        transform.LookAt(shootingTargetTransform, shootingTargetTransform.up);
        transform.rotation = Quaternion.Slerp(currentRotation, transform.rotation, Time.deltaTime);
    }

    private void Shoot()
    {
        bullet.ShootRay(damage, accuracy);

        Invoke("Shoot", fireRate);

        //Debug.Log("bang");
    }

    //##########################################################################################//
    // Dying methods                                                                            //
    //##########################################################################################//

    private void Die()
    {
        gameObject.SetActive(false);
    }
}
