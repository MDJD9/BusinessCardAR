using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using Random = UnityEngine.Random;

public class LaunchCube : MonoBehaviour
{
    private float forceAmount = 0.5f; // Strength of force applied
    private bool _wasTouchingLastFrame = false;
    private Camera _camera;
    private float totalForce;
    private void Start()
    {
        _camera = Camera.main;
        totalForce = 0;
    }

    private void Update()
    {
        if (Touchscreen.current == null) return;  // Safety check for null reference

        bool isTouching = Touchscreen.current.primaryTouch.press.ReadValue() >= 1.0f;
        
        if (isTouching) // Detects the first frame of touch
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            Ray ray = _camera.ScreenPointToRay(touchPosition);

            RaycastHit[] hits = Physics.RaycastAll(ray, 200f);

            foreach (RaycastHit hit in hits)
            {                
                Debug.Log("ARProjecto - " + hit.collider.gameObject.name);
                if (hit.collider.CompareTag("Items"))
                {
                    totalForce += forceAmount * Time.deltaTime;
                }
            }
        }

        if (!isTouching && _wasTouchingLastFrame)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            ARTrackedImageManager trackedImageManager = FindAnyObjectByType<ARTrackedImageManager>();

            if (trackedImageManager != null && trackedImageManager.trackables.count > 0)
            {
                ARTrackedImage closestImage = null;
                float minDistance = float.MaxValue;

                foreach (ARTrackedImage image in trackedImageManager.trackables)
                {
                    float distance = Vector3.Distance(image.transform.position, transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestImage = image;
                    }
                }

                if (closestImage != null)
                {
                    rb.AddForce(closestImage.transform.forward * totalForce, ForceMode.Impulse);
                    totalForce = 0;
                }
            }
        }
        _wasTouchingLastFrame = isTouching;
    }

    // private void CheckTap(Vector2 screenPosition)
    // {
    //     Ray ray = Camera.main.ScreenPointToRay(screenPosition);
    //     if (Physics.Raycast(ray, out RaycastHit hit))
    //     {
    //         if (hit.collider.gameObject == gameObject) // If the tapped object is this ball
    //         {
    //             AddRandomForce();
    //         }
    //     }
    // }
    //
    // private void AddRandomForce()
    // {
    //     Rigidbody rb = GetComponent<Rigidbody>();
    //     if (rb != null)
    //     {
    //         Vector3 randomDirection = Random.insideUnitSphere.normalized; // Random direction
    //         rb.AddForce(randomDirection * forceAmount, ForceMode.Impulse);
    //     }
    // }
}
