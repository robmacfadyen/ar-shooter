using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SceneLoader sceneLoader = GameObject.Find("SceneController").GetComponent<SceneLoader>();

        sceneLoader.BindStart();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
