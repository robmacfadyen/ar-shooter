﻿using System.Collections;
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
    private Text bulletsText;

    [SerializeField]
    private Text debugText;

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
        RaycastHit[] targets = Physics.RaycastAll(camera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2))).OrderBy(h => h.distance).ToArray();

        // check if the first hit is an enemy
        if (targets.Length > 0)
        {
            GameObject firstTarget = targets[0].collider.gameObject;
            if (firstTarget.GetComponent<HealthBehaviour>() != null)
            {
                // damage the target
                firstTarget.GetComponent<HealthBehaviour>().Hit(damage);
                debugText.text = "Hit an enemy";
            }
            else
            {
                debugText.text = "Hit a non-enemy";
            }
        }
        else
        {
            debugText.text = "Hit nothing";
        }
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