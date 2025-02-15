using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PushBalls : MonoBehaviour
{
    public float forceAmount = 0.005f; // Strength of force applied
    private bool _wasTouchingLastFrame = false;
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Touchscreen.current == null) return;  // Safety check for null reference

        bool isTouching = Touchscreen.current.primaryTouch.press.ReadValue() >= 1.0f;

        if (isTouching && !_wasTouchingLastFrame) // Detects the first frame of touch
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            Ray ray = _camera.ScreenPointToRay(touchPosition);

            RaycastHit[] hits = Physics.RaycastAll(ray, 200f);

            foreach (RaycastHit hit in hits)
            {                
                Debug.Log("ARProjecto - " + hit.collider.gameObject.name);
                if (hit.collider.CompareTag("Items"))
                {
                    Rigidbody rb = hit.collider.gameObject.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        Debug.Log("ARProjecto - Found rigidbody");
                        Vector3 randomDirection = Random.insideUnitSphere.normalized; // Random direction
                        rb.AddForce(randomDirection * forceAmount, ForceMode.Impulse);
                    }
                    else
                    {
                        Debug.Log("ARProjecto - RigidBody is null");
                    }
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
