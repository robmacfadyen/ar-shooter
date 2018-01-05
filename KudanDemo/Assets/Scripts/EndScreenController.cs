using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreenController : MonoBehaviour {

    [SerializeField]
    private Button MenuButton;
    [SerializeField]
    private Text text;

	// Use this for initialization
	void Start () {
        SceneLoader sceneLoader = GameObject.Find("SceneController").GetComponent<SceneLoader>();

        sceneLoader.BindEnd();

        if (sceneLoader.result)
        {
            text.text = "The base was defended\nTotal score ";
        }
        else
        {
            text.text = "The base was destroyed\nTotal score ";
        }
        text.text += sceneLoader.allTimeScore;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
