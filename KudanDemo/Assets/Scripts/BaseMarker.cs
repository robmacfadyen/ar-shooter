using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;

public class BaseMarker : MonoBehaviour {
    private Vector3 worldTransform;
    private Camera camera;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
        transform.position = camera.WorldToScreenPoint(worldTransform);
        transform.localScale = Vector3.one;
	}

    public void ShowBase(float lat, float lon, int level)
    {
        AbstractMap map = GameObject.Find("Map").GetComponent<AbstractMap>();
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        GetComponentInChildren<Text>().text = level.ToString();

        Vector2d worldPosition = Conversions.GeoToWorldPosition(lat, lon, map.CenterMercator, map.WorldRelativeScale);

        Debug.Log(lat + "," + lon);
        Debug.Log(map.WorldRelativeScale);
        Debug.Log(map.CenterLatitudeLongitude.x + "," + map.CenterLatitudeLongitude.y);
        Debug.Log(worldPosition.x + "," + worldPosition.y);

        worldTransform = new Vector3((float)worldPosition.x, 0, (float)worldPosition.y);
    }
}
