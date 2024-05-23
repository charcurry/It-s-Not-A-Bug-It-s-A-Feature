using UnityEngine;

public class AutoScrollCredits : MonoBehaviour
{
    public RectTransform creditsContent; // Reference to the CreditsContent RectTransform
    public float scrollSpeed = 20f; // Speed of the scrolling

    private Vector3 vecOriginalLocalPosition;
    private float startPositionY;

    void Start()
    {
        vecOriginalLocalPosition = creditsContent.localPosition;
        // Calculate start position (below the visible area)
        //  startPositionY = -GetComponent<RectTransform>().rect.height / 2 - creditsContent.rect.height / 2;
        //  // Calculate end position (above the visible area)
        //  endPositionY = GetComponent<RectTransform>().rect.height / 2 + creditsContent.rect.height / 2;
        // Set initial position
        // creditsContent.localPosition = new Vector3(creditsContent.localPosition.x, startPositionY, creditsContent.localPosition.z);
    }

    public void ResetLocalPosition()
    {
        creditsContent.localPosition = vecOriginalLocalPosition;

    }    

    void Update()
    {
      //  creditsContent.localPosition += new Vector3(0, scrollSpeed * Time.unscaledDeltaTime, 0);
    }
}