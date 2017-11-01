using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private float turn = 30f;

    [SerializeField]
    private bool pathfind = true;
    [SerializeField]
    private Transform targetTransform;
    private bool followTarget = false;
    private SpawnerBehaviour parentSpawner;

	// Use this for initialization
    void Start()
    {
        // gameObject.SetActive(false);
    }

    public void Spawn(Vector3 spawnPos, float spawnAngle, SpawnerBehaviour spawner)
    {
        this.transform.localPosition = spawnPos;
        this.transform.localRotation = Quaternion.AngleAxis(spawnAngle, Vector3.up);
        parentSpawner = spawner;
        followTarget = false;
        Invoke("TrackTarget", Random.Range(1, 6));
    }

	void OnEnable ()
    {
        
    }

    void OnDisable()
    {
        parentSpawner.KillEnemy();
        CancelInvoke();
    }
	
	// Update is called once per frame
	void Update () {
        if (pathfind)
        {
            if (followTarget)
            {
                TargetWalk();
            }
            else
            {
                RandomWalk();
            }
        }
	}

    private void RandomWalk()
    {
        this.transform.localRotation *= Quaternion.AngleAxis(Random.Range(-turn, turn) * Time.deltaTime, Vector3.up);
        this.transform.localPosition += this.transform.localRotation * Vector3.forward * speed * Time.deltaTime;
    }

    private void TrackTarget()
    {
        followTarget = true;
    }

    private void TargetWalk()
    {
        this.transform.LookAt(targetTransform);
        this.transform.localRotation = Quaternion.Euler(0, this.transform.localRotation.eulerAngles.y, 0);
        this.transform.localPosition += this.transform.localRotation * Vector3.forward * speed * Time.deltaTime;
    }
}
