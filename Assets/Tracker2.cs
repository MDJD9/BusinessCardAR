using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Tracker2 : MonoBehaviour
{
    
    [SerializeField] private ARTrackedImageManager _trackedImageManager;
    [SerializeField] private GameObject objectToActivate;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    void OnEnable() => _trackedImageManager.trackablesChanged.AddListener(OnChanged);

    void OnDisable() => _trackedImageManager.trackablesChanged.RemoveListener(OnChanged);
    
    void OnChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        // When a new image is detected
        foreach (var trackedImage in eventArgs.added)
        {
            SpawnObjects(trackedImage);
        }

        // Update existing images
        foreach (var trackedImage in eventArgs.updated)
        {
            UpdateObjectPositionAndRotation(trackedImage);
        }

        // Remove objects if tracking is lost
        foreach (var trackedImage in eventArgs.removed)
        {
            objectToActivate.SetActive(false);
        }    
    }

    Quaternion getRotation(ARTrackedImage trackedImage)
    {
        Quaternion markerRotation = trackedImage.transform.rotation;
        Vector3 gravity = Input.acceleration.normalized;
        Quaternion phoneTiltRotation = Quaternion.FromToRotation(Vector3.up, gravity);
        Quaternion finalRotation = markerRotation * phoneTiltRotation * Quaternion.Euler(0, 180, 0);
    
        return finalRotation;
    }
    void SpawnObjects(ARTrackedImage trackedImage)
    {
        objectToActivate.transform.position = trackedImage.transform.position;
        objectToActivate.transform.rotation = getRotation(trackedImage);
        objectToActivate.SetActive(true);
    }
    
    void UpdateObjectPositionAndRotation(ARTrackedImage trackedImage)
    {
        objectToActivate.transform.position = trackedImage.transform.position;
        objectToActivate.transform.rotation = getRotation(trackedImage);
    }
    
    void CopyComponent<T>(GameObject source, GameObject destination) where T : Component
    {
        // Get the source component
        T sourceComponent = source.GetComponent<T>();

        if (sourceComponent != null)
        {
            // Add the component to the destination object
            T destinationComponent = destination.AddComponent<T>();

            // Use Reflection to copy field values
            FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                if (field.IsPublic || field.GetCustomAttribute<SerializeField>() != null)
                {
                    // Copy value from source to destination
                    field.SetValue(destinationComponent, field.GetValue(sourceComponent));
                }
            }

            Debug.Log("ARProjecto - Component copied successfully!");
        }
        else
        {
            Debug.LogError("ARProjecto - Source GameObject does not have the component!");
        }
    }
}
