using Kudan.AR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerBehaviour : MonoBehaviour {

    // set up a pooling system to replace this
    [SerializeField]
    private EnemyBehaviour[] enemies;
    private int activeEnemies = 0;
    private bool active = false;

    [SerializeField]
    private float spawnRadius = 20;

    [SerializeField]
    private Text debugText;

    [SerializeField]
    private PlaceSpawners placeSpawners;

    void OnEnable()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        debugText.text = "Spawned " + enemies.Length + " enemies at " + Time.time + "s";

        foreach (EnemyBehaviour enemy in enemies)
        {
            enemy.gameObject.SetActive(true);

            //enemy.Spawn(Vector3.zero, Random.Range(0, 360));
            enemy.Spawn(Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up) * (Vector3.forward * Random.Range(0, spawnRadius)), Random.Range(0, 360), this);
            activeEnemies++;
        }
        active = true;
    }

    void OnDisable()
    {
        active = false;

        debugText.text = "Disabled at " + Time.time + "s";

        placeSpawners.KillSpawner();
    }

    // Use this for initialization
    //void Start () {
    //    foreach (EnemyBehaviour enemy in enemies)
    //    {
    //        enemy.gameObject.SetActive(false);
    //    }
    //}
	
	// Update is called once per frame
	void Update () {
        if (active)
        {
            if (activeEnemies <= 0)
            {
                this.gameObject.SetActive(false);
            }
        }
	}

    public void KillEnemy()
    {
        activeEnemies--;
    }
}
