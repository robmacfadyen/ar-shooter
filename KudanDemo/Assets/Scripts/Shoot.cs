using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shoot : MonoBehaviour
{
    private float bullets;
    [SerializeField]
    private float maxBullets = 20;
    [SerializeField]
    private float fireTime = 0.5f;
    [SerializeField]
    private float reloadTime = 1.5f;
    private bool canShoot = true;

    [SerializeField]
    private Color normalColor;
    [SerializeField]
    private Color shootColor;
    [SerializeField]
    private Color reloadColor;


    [SerializeField]
    private Image crosshair;

    [SerializeField]
    private Text bulletsText;

    // Use this for initialization
    void Start()
    {
        Reload();
    }

    public void Trigger()
    {
        if (canShoot && bullets > 0)
        {
            crosshair.color = shootColor;
            canShoot = false;
            Invoke("SetCanShoot", fireTime);

            ShootRay();
        }
    }

    private void SetCanShoot()
    {
        crosshair.color = normalColor;
        canShoot = true;
    }

    private void ShootRay()
    {
        bullets--;

        UpdateBullets();

        // cast a ray from the crosshair

        // return a list of targets hit

        // call Shot method on the first target
    }

    public void Reload()
    {
        bullets = maxBullets;
        UpdateBullets();
        crosshair.color = reloadColor;
        canShoot = false;
        Invoke("SetCanShoot", reloadTime);
    }

    private void UpdateBullets()
    {
        // temporary method for displaying bullets, should change this to make the UI update itself
        bulletsText.text = "Reload (" + bullets + ")";
    }
}