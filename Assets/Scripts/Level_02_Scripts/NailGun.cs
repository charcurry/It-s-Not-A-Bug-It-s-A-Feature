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
    [SerializeField] private float destroyTime = 2f;

    [Header("Player Properties")]
    [SerializeField] private Camera playerCamera;

    [Header("Sway Properties")]
    [SerializeField] private float swayAmount = 0.02f;
    [SerializeField] private float swaySmoothness = 4f;

    private Vector3 originalPosition;
    private Vector3 swayPosition;

    private void Start()
    {
        originalPosition = transform.localPosition;
    }

    private void Update()
    {
        HandleSway();

        if (Input.GetButtonDown("Fire2"))
        {
            Fire();
            SoundManager.PlaySound(SoundManager.Sound.Nail_Gun, transform.position);
        }

        // Smoothly return the nail gun to its original position after recoil
        transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition + swayPosition, Time.deltaTime * recoilSpeed);
    }

    private void HandleSway()
    {
        float swayX = Mathf.Clamp(Input.GetAxis("Mouse X") * swayAmount, -swayAmount, swayAmount);
        float swayY = Mathf.Clamp(Input.GetAxis("Mouse Y") * swayAmount, -swayAmount, swayAmount);

        Vector3 targetSwayPosition = new Vector3(swayX, swayY, 0);
        swayPosition = Vector3.Lerp(swayPosition, targetSwayPosition, Time.deltaTime * swaySmoothness);
    }

    private void Fire()
    {
        // Fire nail projectile
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 fireDirection = playerCamera.transform.forward;
            rb.velocity = fireDirection * projectileSpeed;
        }

        StartCoroutine(DestroyProjectile(projectile, destroyTime));

        ApplyRecoil();
    }

    private IEnumerator DestroyProjectile(GameObject projectile, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(projectile);
    }

    private void ApplyRecoil()
    {
        transform.Translate(-recoilDistance, 0, 0);
    }
}
