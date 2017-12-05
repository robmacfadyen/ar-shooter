using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Shoot : MonoBehaviour
{
    [SerializeField]
    private float damage = 20;
    private float bullets;
    [SerializeField]
    private float maxBullets = 20;
    [SerializeField]
    private float fireTime = 0.5f;
    [SerializeField]
    private float reloadTime = 1.5f;
    private bool canShoot = true;

    [SerializeField]
    private Camera camera;

    [SerializeField]
    private Color normalColor;
    [SerializeField]
    private Color shootColor;
    [SerializeField]
    private Color reloadColor;


    [SerializeField]
    private Image crosshair;

    [SerializeField]
    private AudioSource shootSound;
    [SerializeField]
    private AudioSource dryFireSound;
    [SerializeField]
    private AudioSource reloadSound;

    [SerializeField]
    private Text bulletsText;

    [SerializeField]
    private Text debugText;

    // Use this for initialization
    void Start()
    {
        Reload();
    }

    void Update()
    {
        //crosshair.rectTransform.position = Input.mousePosition;
    }

    public void Trigger()
    {
        if (canShoot)
        {
            if (bullets > 0)
            {
                crosshair.color = shootColor;
                canShoot = false;
                Invoke("SetCanShoot", fireTime);
                shootSound.Play();

                ShootRay();
            }
            else
            {
                dryFireSound.Play();
            }
        }
    }

    private void SetCanShoot()
    {
        crosshair.color = normalColor;
        canShoot = true;

        if (bullets <= 0)
        {
            Reload();
        }
    }

    private void ShootRay()
    {
        bullets--;

        UpdateBullets();

        // cast a ray from the crosshair
        Debug.Log(crosshair.rectTransform.position);
        RaycastHit[] targets = Physics.RaycastAll(camera.ScreenPointToRay(crosshair.rectTransform.position)).OrderBy(h => h.distance).ToArray();

        // check if the first hit is an enemy
        if (targets.Length > 0)
        {
            GameObject firstTarget = targets[0].collider.gameObject;
            if (firstTarget.GetComponent<HealthBehaviour>() != null)
            {
                // damage the target
                firstTarget.GetComponent<HealthBehaviour>().Hit(damage);
            }
            else if (firstTarget.GetComponent<EnemyController>() != null)
            {
                // damage the target
                firstTarget.GetComponent<EnemyController>().Hit(damage);
            }
            //debugText.text = "Hit " + firstTarget.ToString();
        }
        else
        {
            //debugText.text = "Hit nothing";
        }
    }

    public void Reload()
    {
        bullets = maxBullets;
        UpdateBullets();
        crosshair.color = reloadColor;
        reloadSound.Play();
        canShoot = false;
        Invoke("SetCanShoot", reloadTime);
    }

    private void UpdateBullets()
    {
        // temporary method for displaying bullets, should change this to make the UI update itself
        bulletsText.text = "Reload (" + bullets + ")";
    }
}