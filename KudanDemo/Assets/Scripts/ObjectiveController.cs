using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveController : MonoBehaviour {

    [SerializeField]
    private Slider healthBar;
    [SerializeField]
    private Slider buildBar;

    [SerializeField]
    private float maxHealth = 5000f;
    private float health = 5000f;

    [SerializeField]
    private float buildTime = 180f;
    private float builtTime = 0f;

    [SerializeField]
    private MeshRenderer mesh;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        builtTime += Time.deltaTime;
        buildBar.value = builtTime;
	}

    public void Build(float startingHealth, float timeToBuild)
    {
        maxHealth = startingHealth;
        health = maxHealth;

        buildTime = timeToBuild;
        builtTime = 0;

        healthBar.maxValue = maxHealth;
        buildBar.maxValue = buildTime;

        healthBar.value = health;
        buildBar.value = builtTime;
    }

    public void Hit(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }

        mesh.material.color = Color.Lerp(Color.red, Color.white, health / maxHealth);

        Debug.Log(health);

        healthBar.value = health;
    }

    public void Die()
    {

    }

    public bool IsDead() {
        return (health <= 0);
    }

    public bool IsBuilt()
    {
        return (builtTime >= buildTime);
    }
}
