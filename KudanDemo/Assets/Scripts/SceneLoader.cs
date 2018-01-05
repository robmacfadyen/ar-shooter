using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour {

    public int allTimeScore = 0;
    public int roundScore = 0;
    public bool result = true;

    public int activeBase;

    public List<Base> bases = new List<Base>();

    public struct Base
    {
        public float lat;
        public float lon;
        public int lvl;

        public Base(float latitude, float longitude)
        {
            lat = latitude;
            lon = longitude;
            lvl = 0;
        }

        public Base(float latitude, float longitude, int level)
        {
            lat = latitude;
            lon = longitude;
            lvl = level;
        }

        public void levelUp()
        {
            lvl++;
        }

        public float DistanceTo(float lat2, float lon2)
        {
            // haversine formula to calculate distance between coordinates
            float r = 6371000;
            float dLat = Mathf.Deg2Rad * (lat2 - lat);
            float dLon = Mathf.Deg2Rad * (lon2 - lon);
            float a =
                Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) +
                Mathf.Cos(lat * Mathf.Deg2Rad) * Mathf.Cos(lat2 * Mathf.Deg2Rad) *
                Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2);
            float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
            float d = r * c;
            return d;
        }
    }

    public void SetActiveBase(float lat, float lon)
    {
        activeBase = -1;
        float distance = 330;

        for (int i = 0; i < bases.Count; i++)
        {
            Debug.Log(bases[i].DistanceTo(lat, lon));

            if (bases[i].DistanceTo(lat, lon) < distance)
            {
                activeBase = i;
                distance = bases[i].DistanceTo(lat, lon);
            }
        }
    }

    public void NewBase(float lat, float lon)
    {
            activeBase = bases.Count;
            bases.Add(new Base(lat,lon));
    }

	// Use this for initialization
	void Awake () {
        DontDestroyOnLoad(gameObject);
        Debug.Log(new Base(35.684045f, 139.774490f).DistanceTo(51.513377f, -0.088895f));

        //PlayerPrefs.SetFloat("lat0", 50.385121f);
        //PlayerPrefs.SetFloat("lon0", -4.156739f);
        //PlayerPrefs.SetInt("lvl0", 1);
        //PlayerPrefs.SetFloat("lat1", 50.370033f);
        //PlayerPrefs.SetFloat("lon1", -4.145885f);
        //PlayerPrefs.SetInt("lvl1", 3);

        Load();
        MainMenu();

	}


	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartMap()
    {
        Load();
        SceneManager.LoadScene("Mapbox/Examples/SlippyTerrain/Slippy");
    }

    public void BindStart()
    {
        Load();

        GameObject.Find("PlayMapButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("PlayMapButton").GetComponent<Button>().onClick.AddListener(StartMap);

        if (GameObject.Find("TitleScoreText"))
        {
            GameObject.Find("TitleScoreText").GetComponent<Text>().text = "Score " + allTimeScore;
        }
    }

    public void BindMap()
    {
        GameObject.Find("PlayButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("PlayButton").GetComponent<Button>().onClick.AddListener(StartGame);
        GameObject.Find("BackButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("BackButton").GetComponent<Button>().onClick.AddListener(MainMenu);
    }

    public void BindEnd()
    {
        GameObject.Find("PlayButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("PlayButton").GetComponent<Button>().onClick.AddListener(MainMenu);
        Debug.Log("bound end");
    }

    public void StartGame()
    {

        roundScore = 0;
        //SceneManager.LoadScene("Scenes/Defend");
        SceneManager.LoadScene("KudanAR/Samples/KudanSample");
        //SceneManager.LoadScene("Scenes/EndGame");

        //if (GameObject.Find("Kudan Bundle - With UI"))
        //{
        //    GameObject.Find("Kudan Bundle - With UI").SetActive(true);
        //}
    }

    public void MainMenu()
    {
        //if (GameObject.Find("Kudan Bundle - With UI"))
        //{
        //    GameObject.Find("Kudan Bundle - With UI").SetActive(false);
        //}
        Debug.Log("title");
        SceneManager.LoadScene("Scenes/Title");
        
    }

    public void EndGame(int score, bool win)
    {
        // remove last base if no points
        if (win)
        {
            bases[activeBase].levelUp();
        }

        result = win;

        roundScore = score;
        SaveScore();
        Save();
        SceneManager.LoadScene("Scenes/EndGame");
    }

    public void SaveScore()
    {
        allTimeScore += roundScore;
        roundScore = 0;
    }

    public void AddScore(int score)
    {
        roundScore += score;
    }

    public void Load()
    {
        allTimeScore = PlayerPrefs.GetInt("score", 0);
        roundScore = 0;

        // load all locations that the player has player in
        bases = new List<Base>();
        int index = 0;
        while (PlayerPrefs.HasKey("lat" + index))
        {
            bases.Add(new Base(PlayerPrefs.GetFloat("lat" + index), PlayerPrefs.GetFloat("lon" + index), PlayerPrefs.GetInt("lvl" + index)));
            index++;
        }
    }

    public void Save()
    {
        PlayerPrefs.SetInt("score", allTimeScore);

        for (int i = 0; i < bases.Count; i++)
        {
            if (bases[i].lvl > 0)
            {
                PlayerPrefs.SetFloat("lat" + i, bases[i].lat);
                PlayerPrefs.SetFloat("lon" + i, bases[i].lon);
                PlayerPrefs.SetInt("lvl" + i, bases[i].lvl);
            }
        }

        Load();
    }
}
