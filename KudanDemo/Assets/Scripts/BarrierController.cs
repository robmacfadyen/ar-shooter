using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierController : MonoBehaviour {

    [SerializeField]
    private float health = 1000f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Awake()
    {

    }

    public void Hit(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }

        Debug.Log(health);
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }
}
