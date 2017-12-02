using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletController : MonoBehaviour {

    [SerializeField]
    private Transform gunTransform;
    private TrailRenderer trail;

    private Vector3 targetPosition;

	// Use this for initialization
	void Start () {
        trail = GetComponent<TrailRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void ShootRay(float damage, float accuracy)
    {
        //bullets--;

        //UpdateBullets();
        transform.position = gunTransform.position;
        trail.Clear();


        // cast a ray from the crosshair
        RaycastHit[] targets = Physics.RaycastAll(gunTransform.position, gunTransform.forward, 500, (1<<8)).OrderBy(h => h.distance).ToArray();

        // check if the first hit is an enemy
        if (targets.Length > 0)
        {
            GameObject firstTarget = targets[0].collider.gameObject;
            if (firstTarget.GetComponent<ObjectiveController>() != null)
            {
                targetPosition = targets[0].point;
                Invoke("GoToTarget", 0.05f);

                // damage the target
                firstTarget.GetComponent<ObjectiveController>().Hit(damage);
                //debugText.text = "Hit an enemy";
                Debug.Log("Hit the objective");
            }
            else if (firstTarget.GetComponent<BarrierController>() != null)
            {
                targetPosition = targets[0].point;
                Invoke("GoToTarget", 0.05f);

                // damage the target
                firstTarget.GetComponent<BarrierController>().Hit(damage);
                //debugText.text = "Hit an enemy";
                Debug.Log("Hit a barrier");
            }
            else
            {
                //debugText.text = "Hit a non-enemy";
                Debug.Log("Hit something else");
            }
        }
        else
        {
            //debugText.text = "Hit nothing";
            Debug.Log("Hit nothing");
        }
    }

    private void GoToTarget()
    {
        transform.position = targetPosition;
    }
}
