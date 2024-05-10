using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityCatcher : MonoBehaviour
{
    public Transform eTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (eTarget != null)
        {
            other.transform.position = eTarget.position;
        }
    }
}
