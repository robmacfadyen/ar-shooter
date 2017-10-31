using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerBehaviour : MonoBehaviour {

    // set up a pooling system to replace this
    [SerializeField]
    private EnemyBehaviour[] enemies;

    [SerializeField]
    private float spawnRadius = 20;

    [SerializeField]
    private Text debugText;

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
            enemy.Spawn(Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up) * (Vector3.forward * Random.Range(0, spawnRadius)), Random.Range(0, 360));
        }
    }

    void OnDisable()
    {
        debugText.text = "Disabled at " + Time.time + "s";
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
		
	}
}
