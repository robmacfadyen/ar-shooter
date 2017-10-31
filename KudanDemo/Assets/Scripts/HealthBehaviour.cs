using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBehaviour : MonoBehaviour {
    [SerializeField]
    private float maxHealth;
    private float health;



	// Use this for initialization
	void OnEnable() {
        health = maxHealth;
	}

    public void Hit(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Die()
    {
        gameObject.SetActive(false);
    }

    public float Health
    {
        get
        {
            return health;
        }
    }

    public bool Alive
    {
        get
        {
            return (health >= 0);
        }
    }
}
