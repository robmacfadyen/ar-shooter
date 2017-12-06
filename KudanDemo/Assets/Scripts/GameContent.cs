using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameContent : MonoBehaviour {

    [SerializeField]
    private PoolController pool;
    [SerializeField]
    private ObjectiveController objective;

    [SerializeField]
    private int enemyCount = 100;
    [SerializeField]
    private float minEnemySpawn = 15f;
    [SerializeField]
    private float maxEnemySpawn = 20f;
    [SerializeField]
    private int minWaveSize = 2;
    [SerializeField]
    private int maxWaveSize = 6;
    [SerializeField]
    private int minEnemiesOnScreen = 2;
    [SerializeField]
    private float maxTimeBetweenWaves = 15f;

    [SerializeField]
    public Text debugText;

    [SerializeField]
    private PlaceSpawners placeSpawners;

    private bool active = false;
    private float enemies = 0;

	// Use this for initialization
	void OnEnable() {
        //debugText.text += "Enabled";
        Init();
	}

    public void Init()
    {
        if (!active)
        {
            CreatePlayField();

            SpawnCycle();
        }
    }

    //void OnDisable()
    //{
    //    DestroyPlayField();
    //}
	
	// Update is called once per frame
	void Update() {
        if (active)
        {
            if (objective.IsDead())
            {
                DestroyPlayField();
                placeSpawners.Lose();
                debugText.text = "Objective is dead";
            }

            if (objective.IsBuilt())
            {
                DestroyPlayField();
                placeSpawners.Win();
            }

            if (enemies < minEnemiesOnScreen)
            {
                CancelInvoke("SpawnCycle");
                SpawnCycle();
            }
        }
	}

    void SpawnCycle()
    {
        int numberToSpawn = Random.Range(minWaveSize, maxWaveSize + 1);
        SpawnEnemies(Random.Range(0f, 360f), Random.Range(7f, 9f), 1f, numberToSpawn);
        minWaveSize++;
        maxWaveSize++;
        Invoke("SpawnCycle", maxTimeBetweenWaves);
    }

    void CreatePlayField()
    {
        objective.Build(800f, 150f);

        pool.CreatePool();

        for (int i = 0; i < 8; i++)
        {
            
            GameObject obj = pool.CreateObject(0); // create a barrier
            obj.transform.localRotation = Quaternion.AngleAxis(i * 45f, Vector3.up);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.Translate(Vector3.forward * 2.4f * obj.transform.lossyScale.x);
            obj.GetComponent<BarrierController>().Build(100f);

            //debugText.text = obj.ToString();
        }

        for (int i = 0; i < 12; i++)
        {

            GameObject obj = pool.CreateObject(0); // create a barrier
            obj.transform.localRotation = Quaternion.AngleAxis(i * 30f, Vector3.up);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.Translate(Vector3.forward * 3.6f * obj.transform.lossyScale.x);
            obj.GetComponent<BarrierController>().Build(100f);

            //debugText.text = obj.ToString();
        }

        for (int i = 0; i < 16; i++)
        {

            GameObject obj = pool.CreateObject(0); // create a barrier
            obj.transform.localRotation = Quaternion.AngleAxis(i * 22.5f, Vector3.up);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.Translate(Vector3.forward * 4.8f * obj.transform.lossyScale.x);
            obj.GetComponent<BarrierController>().Build(100f);

            //debugText.text = obj.transform.localPosition.ToString();
        }

        active = true;
    }

    void DestroyPlayField()
    {
        active = false;
    }

    void SpawnEnemies(float angle, float distance, float spread, int number)
    {
        for (int i = 0; i < number; i++)
        {
            GameObject obj = pool.CreateObject(1); // create an enemy
            if (obj)
            {
                obj.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.up);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.Translate(Vector3.forward * distance * obj.transform.lossyScale.x);
                obj.transform.localPosition += new Vector3(Random.Range(-spread, spread), 0, Random.Range(-spread, spread));

                obj.GetComponent<EnemyController>().Init();

                enemies++;
            }
        }
    }

    public void KillEnemy()
    {
        enemies--;
    }
}
