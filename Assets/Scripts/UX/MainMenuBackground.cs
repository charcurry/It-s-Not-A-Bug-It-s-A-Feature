using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuBackground : MonoBehaviour
{
    [SerializeField] private RawImage image;
    [SerializeField] private float x, y;

    void Update()
    {
        image.uvRect = new Rect(image.uvRect.position + new Vector2(x, y), image.uvRect.size);
    }
}
