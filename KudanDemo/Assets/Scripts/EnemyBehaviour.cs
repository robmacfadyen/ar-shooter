﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

    private float speed = 0.01f;
    private float turn = 10f;

	// Use this for initialization
    void Start()
    {
        // gameObject.SetActive(false);
    }

	void OnEnable ()
    {
        
    }
	
	// Update is called once per frame
	void Update () {
        RandomWalk();
	}

    private void RandomWalk()
    {
        this.transform.localRotation *= Quaternion.AngleAxis(Random.Range(-turn, turn) * Time.deltaTime, Vector3.up);
        this.transform.localPosition += this.transform.localRotation * Vector3.forward * speed * Time.deltaTime;
    }
}
