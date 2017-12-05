using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarrierController : MonoBehaviour {

    [SerializeField]
    private float maxHealth = 100f;

    private float health = 100f;

    [SerializeField]
    private MeshRenderer mesh;

    //[SerializeField]
    //private Text debugText;

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

    public void Build(float startingHealth)
    {
        maxHealth = startingHealth;
        health = maxHealth;
    }

    public void Hit(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }

        mesh.material.color = Color.Lerp(Color.red, Color.white, health / maxHealth);

        //Debug.Log(health);
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }
}
