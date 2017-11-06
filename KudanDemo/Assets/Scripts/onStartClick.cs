using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class onStartClick : MonoBehaviour {

    public delegate void Click();
    public string levelName = "";
	public void onClick()
    {
        SceneManager.LoadScene(levelName);
    }
}
