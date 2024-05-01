using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UX_Manager : MonoBehaviour
{
    // Start is called before the first frame update
    public CanvasScaler canvasScaler;

    void Start()
    {
       /// canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
       // canvasScaler.referenceResolution = new Vector2(1920, 1080); // Set this to your targeted resolution
        //canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
       // canvasScaler.matchWidthOrHeight = 0.5f; // Adjust this value to find what works best for your game
      //  Screen.fullScreen = true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
