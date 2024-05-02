using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util_MakeSingleton : MonoBehaviour
{
    // Start is called before the first frame update

    static bool bInitialized = false;
    void Start()
    {
        if (!bInitialized)
        {
            GameObject.DontDestroyOnLoad(gameObject);
            bInitialized = true;
        }
        else
            GameObject.Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
