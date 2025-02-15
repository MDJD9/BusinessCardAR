using UnityEngine;

public class SwipeHandler : MonoBehaviour
{
    private Camera arCamera;
    private StressReceiver _stressReceiver;
    void Start()
    {
        arCamera = Camera.main;
        SwipeDetector swipeDetector = GetComponent<SwipeDetector>();
        if (swipeDetector != null)
        {
            swipeDetector.OnSwipeDetected = HandleSwipe;
        }
        else
        {
            Debug.LogWarning("GESTURE SSwipe Detector could not be found");
        }
        
        _stressReceiver = arCamera.GetComponent<StressReceiver>();
        if (swipeDetector == null)
        {
            Debug.LogWarning("GESTURE: Stress receiver could not be found");
        }
    }

    void HandleSwipe(Vector2 swipeDirection)
    {
        // Execute your custom logic here, such as moving or animating the object.
        Debug.Log("GESTURE HANDLER Swipe detected with direction: " + swipeDirection);
        _stressReceiver.InduceStress(1.0f);
    }
}