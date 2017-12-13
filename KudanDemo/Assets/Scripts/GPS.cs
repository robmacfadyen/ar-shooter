using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GPS : MonoBehaviour
{
    [SerializeField]
    private Text debugText;

    IEnumerator Start()
    {

        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("not enabled");
            yield break;
        }

        // Start service before querying location
        Input.location.Start();

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
            debugText.text = ("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        }

        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();
    }
}