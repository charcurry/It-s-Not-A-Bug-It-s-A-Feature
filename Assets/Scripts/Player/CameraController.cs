using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float mouseSensitivity = 1f;

    private Transform playerTransform;
    [HideInInspector] public float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = transform.parent.GetComponent<Transform>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, transform.rotation.eulerAngles.z);
        playerTransform.Rotate(Vector3.up * mouseX);
    }
}
