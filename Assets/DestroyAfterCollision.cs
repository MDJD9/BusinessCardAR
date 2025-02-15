using System;
using System.Collections;
using UnityEngine;

public class DestroyAfterCollision : MonoBehaviour
{
    [SerializeField]
    private float timeToDestroyAfterCollision;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            Destroy(this.gameObject, timeToDestroyAfterCollision);    
        }
        
    }

}
