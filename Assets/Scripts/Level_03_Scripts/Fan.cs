using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private bool isFast = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isFast)
            transform.Rotate(Vector3.up * 800 * Time.deltaTime);
        else
            transform.Rotate(Vector3.up * 200 * Time.deltaTime);
    }
}
