using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class ReplacePins : MonoBehaviour
{
    [SerializeField]
    private GameObject pinPrefab;
    [SerializeField]
    private Transform pinPosition;

    [SerializeField] private float time;

    [SerializeField] private GameObject parentObj;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            Debug.Log("SPAWN PINS REPLACING");
            StartCoroutine(ReplaceInGamePins(time));
        }
    }

    IEnumerator ReplaceInGamePins(float time)
    {
        yield return new WaitForSeconds(time);
        var go = Instantiate(pinPrefab, parentObj.transform);
        go.transform.localPosition = pinPosition.localPosition;
        go.transform.localRotation = Quaternion.identity;
        Debug.Log("SPAWN POSITION : " + go.transform.localPosition);
    }

}
