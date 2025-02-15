using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class SwipeDetector : MonoBehaviour
{
    // Define an action to execute when a swipe is detected.
    // The Vector2 parameter can hold the swipe direction.
    public Action<Vector2> OnSwipeDetected;

    private Vector2 startTouchPosition;
    private bool isSwiping = false;
    public float swipeThreshold = 50f;

    void Update()
    {
        if (Touchscreen.current != null)
        {
            var primaryTouch = Touchscreen.current.primaryTouch;

            if (primaryTouch.press.isPressed)
            {
                Vector2 touchPosition = primaryTouch.position.ReadValue();

                // Start of touch
                if (!isSwiping)
                {
                    startTouchPosition = touchPosition;
                    isSwiping = true;
                }
                else
                {
                    Vector2 swipeDelta = touchPosition - startTouchPosition;
                    if (swipeDelta.magnitude > swipeThreshold)
                    {
                        Debug.Log("GESTURE Swipe detected with direction: " + swipeDelta.normalized);
                        OnSwipeDetected?.Invoke(swipeDelta.normalized);
                        isSwiping = false;
                    }
                }
            }
            else
            {
                // Reset when touch is released
                isSwiping = false;
            }
        }
    }
}