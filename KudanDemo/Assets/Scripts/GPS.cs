using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Mapbox.Examples;
using System.Collections.Generic;

public class GPS : MonoBehaviour
{
    [SerializeField]
    private Text debugText;
    //private float mapX = 0;
    //private float mapY = 0;
    [SerializeField]
    private CameraMovement mapControl;
    [SerializeField]
    private Camera camera;
    [SerializeField]
    private RectTransform loadingPanel;
    [SerializeField]
    private Image locationMarker;

    [SerializeField]
    private BaseMarker baseMarkerPrefab;
    [SerializeField]
    private GameObject baseMarkerParent;

    private List<BaseMarker> baseMarkers = new List<BaseMarker>();

    private SceneLoader sceneLoader;

    IEnumerator Start()
    {
        baseMarkers = new List<BaseMarker>();

        sceneLoader = GameObject.Find("SceneController").GetComponent<SceneLoader>();

        for (int i = 0; i < sceneLoader.bases.Count; i++)
        {
            baseMarkers.Add(Instantiate(baseMarkerPrefab));
            baseMarkers[i].transform.parent = baseMarkerParent.transform;
            baseMarkers[i].ShowBase(sceneLoader.bases[i].lat, sceneLoader.bases[i].lon, sceneLoader.bases[i].lvl);
        }

        mapControl.ScrollToLatLon(50.385121f, -4.156739f);

        sceneLoader.BindMap();

        //loadingPanel.gameObject.SetActive(true);

        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("not enabled");
            yield break;
        }

        Input.compass.enabled = true;
        // Start service before querying location
        Input.location.Start(1,1);

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
            Debug.Log(maxWait);
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            debugText.text = ("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            debugText.text = ("Unable to determine device location");
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            debugText.text = ("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.timestamp + " " + Input.compass.enabled);
            mapControl.ScrollToLatLon(Input.location.lastData.latitude, Input.location.lastData.longitude);

            for (int i = 0; i < baseMarkers.Count; i++)
            {
                baseMarkers[i].ShowBase(sceneLoader.bases[i].lat, sceneLoader.bases[i].lon, sceneLoader.bases[i].lvl);
            }

            loadingPanel.gameObject.SetActive(false);
        }

        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();
        loadingPanel.gameObject.SetActive(false);

        
        
    }

    void Update()
    {
        locationMarker.rectTransform.position = camera.WorldToScreenPoint(new Vector3(0, 0, 0));
    }

    public void ShowBases()
    {
        for (int i = 0; i < baseMarkers.Count; i++)
        {
            baseMarkers[i].ShowBase(sceneLoader.bases[i].lat, sceneLoader.bases[i].lon, sceneLoader.bases[i].lvl);
        }

        sceneLoader.SetActiveBase(Input.location.lastData.latitude, Input.location.lastData.longitude);
        //sceneLoader.SetActiveBase(50.37139f, -4.14222f);
        int activeBase = sceneLoader.activeBase;

        if (activeBase == -1)
        {
            sceneLoader.NewBase(Input.location.lastData.latitude, Input.location.lastData.longitude);
            debugText.text = "No bases in your area\nTap PLAY to build a new base";
        }
        else
        {
            int activeLevel = sceneLoader.bases[activeBase].lvl;
            debugText.text = "Level " + activeLevel + " base in your area\nTap PLAY to upgrade the base";
        }
    }
}