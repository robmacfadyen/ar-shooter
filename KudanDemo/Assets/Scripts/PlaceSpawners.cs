using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Kudan.AR;

/// <summary>
/// Script used in the Kudan Samples. Provides functions that switch between different tracking methods and start abitrary tracking.
/// </summary>
public class PlaceSpawners : MonoBehaviour
{
    public KudanTracker _kudanTracker;	// The tracker to be referenced in the inspector. This is the Kudan Camera object.
    public TrackingMethodMarker _markerTracking;	// The reference to the marker tracking method that lets the tracker know which method it is using
    public TrackingMethodMarkerless _markerlessTracking;    // The reference to the markerless tracking method that lets the tracker know which method it is using

    public Text buttonText;

    private int spawnersPlaced = 1;
    private int spawnersKilled = 0;

    public GameContent gameContent;

    public void MarkerClicked()
    {
        _kudanTracker.ChangeTrackingMethod(_markerTracking);	// Change the current tracking method to marker tracking
    }

    public void MarkerlessClicked()
    {
        _kudanTracker.ChangeTrackingMethod(_markerlessTracking);	// Change the current tracking method to markerless tracking
    }

    public void StartClicked()
    {
        if (!_kudanTracker.ArbiTrackIsTracking())
        {
            // from the floor placer.
            Vector3 floorPosition;          // The current position in 3D space of the floor
            Quaternion floorOrientation;    // The current orientation of the floor in 3D space, relative to the device

            _kudanTracker.FloorPlaceGetPose(out floorPosition, out floorOrientation);   // Gets the position and orientation of the floor and assigns the referenced Vector3 and Quaternion those values
            _kudanTracker.ArbiTrackStart(floorPosition, floorOrientation);              // Starts markerless tracking based upon the given floor position and orientations
            spawnersPlaced++;

            gameContent.Init();
        }

        else
        {
            _kudanTracker.ArbiTrackStop();
        }
    }

    public void KillSpawner()
    {
        if (_kudanTracker.ArbiTrackIsTracking())
        {
            _kudanTracker.ArbiTrackStop();
            // buttonText.text = spawnersKilled.ToString();
        }
    }

    public void PlaceSpawner()
    {
        if (!_kudanTracker.ArbiTrackIsTracking())
        {
            // from the floor placer.
            Vector3 floorPosition;          // The current position in 3D space of the floor
            Quaternion floorOrientation;    // The current orientation of the floor in 3D space, relative to the device

            _kudanTracker.FloorPlaceGetPose(out floorPosition, out floorOrientation);   // Gets the position and orientation of the floor and assigns the referenced Vector3 and Quaternion those values
            _kudanTracker.ArbiTrackStart(floorPosition, floorOrientation);              // Starts markerless tracking based upon the given floor position and orientations
            spawnersPlaced++;
        }
    }

    private void Start()
    {
        _kudanTracker.ChangeTrackingMethod(_markerlessTracking);
        //Invoke("PlaceSpawner", 5);
    }

    void Update()
    {
        if (!_kudanTracker.ArbiTrackIsTracking())
        {
            //// buttonText.text = spawnerExists.ToString();
            //if (spawnersPlaced > spawnersKilled)
            //{
            //    spawnersKilled++;
            //    Invoke("checkIfSpawnerIsDead", 1);
                buttonText.text = "Spawn";
            //}
        }
        else
        {
            buttonText.text = "Active";
        }
    }

    private void checkIfSpawnerIsDead()
    {
        if (!_kudanTracker.ArbiTrackIsTracking())
        {
            Invoke("PlaceSpawner", 5);
            buttonText.text = "respawned";
        }
        else
        {
            spawnersKilled--;
            buttonText.text = "not respawned";
        }
    }
}
