using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailGun : MonoBehaviour
{
    [Header("Nail Gun Properties")]
    [SerializeField] private float projectileSpeed = 100f;
    [SerializeField] private float recoilDistance = 0.1f;
    [SerializeField] private float recoilSpeed = 8f;
    [SerializeField] private float destroyTime = 2f;
    [SerializeField] private float fireDelay = 0.2f;

    [Header("Sway Properties")]
    [SerializeField] private float swayAmount = 0.02f;
    [SerializeField] private float swaySmoothness = 4f;

    [Header("Player References")]
    [SerializeField] private Camera playerCamera;

    [Header("References")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    private Vector3 originalPosition;
    private Vector3 swayPosition;
    private bool canFire = true;
    private bool isEnabled = false;
    private MeshRenderer[] meshRenderers;

    private void Start()
    {
        originalPosition = transform.localPosition;
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        HideNailGun();
    }

    private void Update()
    {
        // If not equipped then just do nothing
        if (!isEnabled) return;

        HandleSway();

        if (Input.GetButtonDown("Fire2") && canFire)
        {
            Fire();
            SoundManager.PlaySound(SoundManager.Sound.Nail_Gun, transform.position);
        }

        // Smoothly return the nail gun to its original position after recoil
        transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition + swayPosition, Time.deltaTime * recoilSpeed);
    }

    // Enable the nail gun
    public void EnableNailGun()
    {
        isEnabled = true;
        ShowNailGun();
    }

    // Handle sway of nail gun based on mouse movement
    private void HandleSway()
    {
        float swayX = Mathf.Clamp(Input.GetAxis("Mouse X") * swayAmount, -swayAmount, swayAmount);
        float swayY = Mathf.Clamp(Input.GetAxis("Mouse Y") * swayAmount, -swayAmount, swayAmount);

        Vector3 targetSwayPosition = new Vector3(-swayX, -swayY, 0);
        swayPosition = Vector3.Lerp(swayPosition, targetSwayPosition, Time.deltaTime * swaySmoothness);
    }

    // Fire the nail gun
    private void Fire()
    {
        if (!canFire) return;

        canFire = false;

        // Calculate rotation cause prefab wants to be dumb and not work
        Quaternion correctRotation = firePoint.rotation * Quaternion.Euler(0, 90, 180);
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, correctRotation);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 fireDirection = playerCamera.transform.forward;
            rb.velocity = fireDirection * projectileSpeed;
        }

        StartCoroutine(DestroyProjectile(projectile, destroyTime));
        ApplyRecoil();
        StartCoroutine(CheckCanFire());
    }

    // Destroy the nail after a delay
    private IEnumerator DestroyProjectile(GameObject projectile, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(projectile);
    }

    // Apply recoil to the nail gun
    private void ApplyRecoil()
    {
        transform.Translate(-recoilDistance, 0, 0);
    }

    // Handle fire delay
    private IEnumerator CheckCanFire()
    {
        yield return new WaitForSeconds(fireDelay);
        canFire = true;
    }

    // Not being used
    private void HideNailGun()
    {
        foreach (MeshRenderer renderer in meshRenderers)
        {
            renderer.enabled = false;
        }
    }

    // Show the nail gun
    private void ShowNailGun()
    {
        foreach (MeshRenderer renderer in meshRenderers)
        {
            renderer.enabled = true;
        }
    }
}
