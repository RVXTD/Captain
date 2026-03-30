using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class FireBullet : MonoBehaviour
{
     public GameObject bullet;
    public Transform spawnPoint;
    public float bulletSpeed = 20f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        grabbable.activated.AddListener(Fire);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire(ActivateEventArgs args)
    {
        GameObject spawnBullet = Instantiate(bullet);
        spawnBullet.transform.position = spawnPoint.position;
        spawnBullet.GetComponent<Rigidbody>().linearVelocity = spawnPoint.forward * bulletSpeed;
        Destroy(spawnBullet, 5f);
    }

}
