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
    public Button startButton;
    public Text debugText;

    public GameObject winScreen;
    public GameObject loseScreen;

    private int spawnersPlaced = 1;
    private int spawnersKilled = 0;

    private enum GameState
    {
        READY,
        PLAYING,
        PAUSED,
        WON,
        LOST
    }
    private GameState state;

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
        //debugText.text += "clicked ";
        if (!_kudanTracker.ArbiTrackIsTracking())
        {
            //debugText.text += "started tracking ";
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
            //debugText.text += "already tracking ";
        }

        //else
        //{
        //    _kudanTracker.ArbiTrackStop();
        //}
    }

    public void KillSpawner()
    {
        if (_kudanTracker.ArbiTrackIsTracking())
        {
            _kudanTracker.ArbiTrackStop();
            //debugText.text += "killed spawner ";
            // buttonText.text = spawnersKilled.ToString();
        }
    }

    //public void PlaceSpawner()
    //{
    //    if (!_kudanTracker.ArbiTrackIsTracking())
    //    {
    //        // from the floor placer.
    //        Vector3 floorPosition;          // The current position in 3D space of the floor
    //        Quaternion floorOrientation;    // The current orientation of the floor in 3D space, relative to the device

    //        _kudanTracker.FloorPlaceGetPose(out floorPosition, out floorOrientation);   // Gets the position and orientation of the floor and assigns the referenced Vector3 and Quaternion those values
    //        _kudanTracker.ArbiTrackStart(floorPosition, floorOrientation);              // Starts markerless tracking based upon the given floor position and orientations
    //        state = GameState.PLAYING;
    //    }
    //}

    private void Start()
    {
        _kudanTracker._startOnEnable = true;
        _kudanTracker.ChangeTrackingMethod(_markerlessTracking);
        state = GameState.READY;
        //debugText.text += "started ";
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
            if (state == GameState.PLAYING)
            {
                startButton.gameObject.SetActive(true);
                buttonText.text = "Lost the target! Find a clear space on the ground, and tap to resume";
                state = GameState.PAUSED;
            }
            //}
        }
        else
        {
            startButton.gameObject.SetActive(false);
            //buttonText.text = "Active";
            state = GameState.PLAYING;
        }

        //debugText.text = state.ToString();
    }

    public void Win()
    {
        state = GameState.WON;
        _kudanTracker.ArbiTrackStop();
        //debugText.text += "won ";
        winScreen.SetActive(true);
        Time.timeScale = 0;
        SceneLoader sceneLoader = GameObject.Find("SceneController").GetComponent<SceneLoader>();
        sceneLoader.EndGame(sceneLoader.roundScore, true);
    }

    public void Lose()
    {
        state = GameState.LOST;
        _kudanTracker.ArbiTrackStop();
        //debugText.text += "lost ";
        loseScreen.SetActive(true);
        Time.timeScale = 0;
        SceneLoader sceneLoader = GameObject.Find("SceneController").GetComponent<SceneLoader>();
        sceneLoader.EndGame(sceneLoader.roundScore, true);
    }

    //private void checkIfSpawnerIsDead()
    //{
    //    if (!_kudanTracker.ArbiTrackIsTracking())
    //    {
    //        Invoke("PlaceSpawner", 5);
    //        buttonText.text = "respawned";
    //    }
    //    else
    //    {
    //        spawnersKilled--;
    //        buttonText.text = "not respawned";
    //    }
    //}
}
