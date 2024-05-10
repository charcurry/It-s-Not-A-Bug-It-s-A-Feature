using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailGun : MonoBehaviour
{
    public float fireDelay = 0.2f;

    public Transform nailgunTransform;
    public float recoilDistance = 0.1f;
    public float recoilSpeed = 8f;
    private Vector3 originalPosition;

    public Transform firePoint;
    public GameObject nailPrefab;
    public float projectileForce = 20f;

    private float nextFireTime = 0f;

    void Start()
    {
        originalPosition = nailgunTransform.localPosition;
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireDelay;
        }

        nailgunTransform.localPosition = Vector3.Lerp(nailgunTransform.localPosition, originalPosition, Time.deltaTime * recoilSpeed);
    }

    void Shoot()
    {
        GameObject projectile = Instantiate(nailPrefab, firePoint.position, firePoint.rotation);
        Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.AddForce(firePoint.forward * projectileForce, ForceMode.Impulse);

        nailgunTransform.localPosition -= Vector3.forward * recoilDistance;
    }
}
