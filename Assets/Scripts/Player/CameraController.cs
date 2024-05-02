using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject uxMainObject;
    private Shared_UXVariables uxVariables;

    private Transform playerTransform;
    [HideInInspector] public float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        uxMainObject = GameObject.Find("UX_Main");
        uxVariables = uxMainObject.GetComponent<Shared_UXVariables>();

        playerTransform = transform.parent.GetComponent<Transform>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * uxVariables.flMouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * uxVariables.flMouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, transform.rotation.eulerAngles.z);
        playerTransform.Rotate(Vector3.up * mouseX);
    }
}
