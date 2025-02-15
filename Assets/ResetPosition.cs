using System;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    [SerializeField] private GameObject positionObject;
    [SerializeField] private GameObject prefabToUse;
    [SerializeField] private float distanceToReset = 1.0f;
    private GameObject currentObject;
    private Rigidbody rb;
    private GameObject copyObj;
    void Start()
    {
        currentObject = Instantiate(prefabToUse, positionObject.transform.position, Quaternion.identity);
        currentObject.transform.localScale = new Vector3(1.0f,1.0f,1.0f);

        rb = currentObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        float distance = Vector3.Distance(currentObject.transform.position, new Vector3(positionObject.transform.position.x, positionObject.transform.position.y, positionObject.transform.position.z));
        Debug.Log("ARProjecto - " + distance);

        if (distance > 1.0f)
        {
            Debug.Log("Reset Distancia: " + distance);
            Destroy(currentObject);
                currentObject = Instantiate(prefabToUse, new Vector3(positionObject.transform.position.x, 
                        positionObject.transform.position.y, 
                        positionObject.transform.position.z), 
                        Quaternion.identity);
                currentObject.gameObject.transform.SetParent(positionObject.transform, true);
                currentObject.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
                rb = currentObject.GetComponent<Rigidbody>();
        }
    }
}