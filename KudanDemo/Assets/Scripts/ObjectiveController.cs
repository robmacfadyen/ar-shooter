using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveController : MonoBehaviour {

    [SerializeField]
    private Slider healthBar;

    [SerializeField]
    private float health = 5000f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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

    }
}
