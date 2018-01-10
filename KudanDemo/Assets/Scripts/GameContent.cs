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
    public Text scoreText;

    [SerializeField]
    private PlaceSpawners placeSpawners;

    private bool active = false;
    private float enemies = 0;

    private SceneLoader sceneLoader;
    private int level = -1;

	// Use this for initialization
	void OnEnable() {
        debugText.text += "Enabled";
        Debug.Log("enabled");
        Init();
	}

    public void Init()
    {
        Debug.Log("init");
        Debug.Log(active);
        scoreText.text = "Score 0";

        sceneLoader = GameObject.Find("SceneController").GetComponent<SceneLoader>();
        level = sceneLoader.bases[sceneLoader.activeBase].lvl;
        debugText.text += level.ToString();

        if (!active)
        {
            CreatePlayField();

            SpawnCycle();
        }

        Debug.Log("forcing win in 3s");
        //Invoke("ForceWin", 3);
    }

    //void OnDisable()
    //{
    //    DestroyPlayField();
    //}
	
	// Update is called once per frame
	void Update() {
        //DestroyPlayField();
        //sceneLoader.EndGame(sceneLoader.roundScore, true);
        //placeSpawners.Win();

        if (active)
        {
            if (objective.IsDead())
            {
                Debug.Log("objective is dead");
                CancelInvoke();
                DestroyPlayField();
                placeSpawners.Lose();
            }

            if (objective.IsBuilt())
            {
                Debug.Log("objective is built");
                CancelInvoke();
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

    void ForceWin()
    {
        Debug.Log("forcewin");
        CancelInvoke();
        DestroyPlayField();
        placeSpawners.Lose();
    }

    void SpawnCycle()
    {
        int numberToSpawn = (int) (Random.Range(minWaveSize, maxWaveSize + 1) / 3);
        SpawnEnemies(Random.Range(0f, 360f), Random.Range(7f, 9f), 3f, numberToSpawn);
        minWaveSize++;
        maxWaveSize++;
        Invoke("SpawnCycle", maxTimeBetweenWaves / 2);
    }

    void CreatePlayField()
    {
        objective.Build(800f, 1f + 90f * level);

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
        pool.DeletePool();
        active = false;
    }

    void SpawnEnemies(float angle, float distance, float spread, int number)
    {
        for (int i = 0; i < number; i++)
        {
            GameObject obj = pool.CreateObject(1); // create an enemy
            if (obj)
            {
                obj.transform.localRotation = Quaternion.AngleAxis(angle + Random.Range(-40f, 40f), Vector3.up);
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
        sceneLoader.AddScore(100);
        scoreText.text = "Score " + sceneLoader.roundScore;
        enemies--;
    }
}
