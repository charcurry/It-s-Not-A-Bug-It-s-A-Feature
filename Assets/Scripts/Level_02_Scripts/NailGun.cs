using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailGun : MonoBehaviour
{
    [Header("Nail Gun Properties")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 100f;
    [SerializeField] private float recoilDistance = 0.1f;
    [SerializeField] private float recoilSpeed = 8f;

    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.localPosition;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }

        // Smoothly return the nail gun to its original position after recoil
        transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * recoilSpeed);
    }

    private void Fire()
    {
        // Fire nail projectile
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = firePoint.forward * projectileSpeed;
        }

        ApplyRecoil();
    }

    private void ApplyRecoil()
    {
        transform.Translate(-recoilDistance, 0, 0);
    }
}
