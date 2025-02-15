using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Tracker : MonoBehaviour
{
    
    [SerializeField] private ARTrackedImageManager _trackedImageManager;
    [SerializeField] private GameObject _prefabToSpawn;
    [SerializeField] private GameObject[] _newObjectsToSpawn;
    [SerializeField] private Vector3[] _objectSpawnPositions;
    [SerializeField] private Quaternion[] _objectSpawnRotations;
    [SerializeField] private Vector3[] _objectSpawnScales;
    [SerializeField] private float _height = 0.001f;
    [SerializeField] private GameObject interactionsPrefab;
    
    private Dictionary<string, GameObject> spawnedObjects;
    private Dictionary<string, List<GameObject>> additionalObjects;
    private GameObject parentObject;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnedObjects = new Dictionary<string, GameObject>();
        additionalObjects = new Dictionary<string, List<GameObject>>();
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
            if (spawnedObjects.ContainsKey(trackedImage.referenceImage.name))
            {
                UpdateObjectPositionAndRotation(trackedImage);
                UpdateAdditionObjectsPositionAndRotation(trackedImage);
            }
        }

        // Remove objects if tracking is lost
        foreach (var trackedImage in eventArgs.removed)
        {
            if (spawnedObjects.ContainsKey(trackedImage.Value.referenceImage.name))
            {
                Destroy(spawnedObjects[trackedImage.Value.referenceImage.name]);
                List<GameObject> objs = additionalObjects[trackedImage.Value.referenceImage.name];
                foreach (var obj in objs)
                {
                    Destroy(obj);
                }
                additionalObjects.Remove(trackedImage.Value.referenceImage.name);
                spawnedObjects.Remove(trackedImage.Value.referenceImage.name);
            }
        }    
    }
    
    void SpawnObjects(ARTrackedImage trackedImage)
    {
        if (!spawnedObjects.ContainsKey(trackedImage.referenceImage.name))
        {
            spawnMainObject(_prefabToSpawn, trackedImage);
            spawnAllOtherObjects(trackedImage);
        }
    }

    private void spawnAllOtherObjects(ARTrackedImage trackedImage)
    {
        for (int i = 0; i <_newObjectsToSpawn.Length; i++)
        {
            _prefabToSpawn = _newObjectsToSpawn[i];
            Quaternion markerRotation = trackedImage.transform.rotation;
            Vector3 gravity = Input.acceleration.normalized;
            Quaternion phoneTiltRotation = Quaternion.FromToRotation(Vector3.up, gravity);
            Quaternion finalRotation = markerRotation * phoneTiltRotation;
            
            GameObject newGameObject = Instantiate(_prefabToSpawn, 
                _objectSpawnPositions[i], 
                _objectSpawnRotations[i]);
            newGameObject.transform.position += trackedImage.transform.position;
            newGameObject.transform.rotation = finalRotation;
            newGameObject.transform.SetParent(trackedImage.transform, true);
            newGameObject.transform.localScale = _objectSpawnScales[i];
            List<GameObject> objs = additionalObjects[trackedImage.referenceImage.name];
            if (objs == null || objs.Count == 0)
            {
                objs = new List<GameObject>();
                additionalObjects[trackedImage.referenceImage.name] = objs;
            }
            objs.Add(newGameObject);
        }
    }


    private  void spawnMainObject(GameObject prefab, ARTrackedImage trackedImage)
    {
        Quaternion markerRotation = trackedImage.transform.rotation;
        Vector3 gravity = Input.acceleration.normalized;
        Quaternion phoneTiltRotation = Quaternion.FromToRotation(Vector3.up, gravity);
        Quaternion finalRotation = markerRotation * phoneTiltRotation * Quaternion.Euler(0, 180, 0);
            
        GameObject newGameObject = Instantiate(_prefabToSpawn, trackedImage.transform.position, finalRotation);
        newGameObject.transform.SetParent(trackedImage.transform, true);
        Vector2 imageSize = trackedImage.referenceImage.size;
            
        newGameObject.transform.localScale = new Vector3(imageSize.x, _height, imageSize.y);
        newGameObject.tag = "MeBox";
        CopyComponent<Interactions>(interactionsPrefab, newGameObject);
        Collider newCollider = newGameObject.AddComponent<BoxCollider>(); 
        newCollider.isTrigger = true; 
        spawnedObjects[trackedImage.referenceImage.name] = newGameObject;
    }
    
    void UpdateObjectPositionAndRotation(ARTrackedImage trackedImage)
    {
        GameObject plane = spawnedObjects[trackedImage.referenceImage.name];
        plane.transform.position = trackedImage.transform.position;
        plane.transform.rotation = trackedImage.transform.rotation * Quaternion.Euler(0, 180, 0);;
    }

    private void UpdateAdditionObjectsPositionAndRotation(ARTrackedImage trackedImage)
    {
        List<GameObject> objs = additionalObjects[trackedImage.referenceImage.name];
        for (int i=0; i<objs.Count; i++)
        {
            var go = objs[i];
        }
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
