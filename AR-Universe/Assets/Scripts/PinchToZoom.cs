using UnityEngine;

public class PinchToZoom : MonoBehaviour
{
    [Header("Zoom Settings")]
    public float zoomSpeed = 0.005f;
    public float minScale = 0.01f;
    public float maxScale = 5.0f;

    void Update()
    {
        // Check if two fingers are on the screen
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Previous frame positions
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Distance between touches in both frames
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Difference in distance
            float deltaMagnitudeDiff = touchDeltaMag - prevTouchDeltaMag;

            // Calculate new scale
            float newScale = transform.localScale.x + (deltaMagnitudeDiff * zoomSpeed);
            newScale = Mathf.Clamp(newScale, minScale, maxScale);

            transform.localScale = new Vector3(newScale, newScale, newScale);
        }
    }
}