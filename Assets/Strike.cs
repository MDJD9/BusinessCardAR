using System;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class Strike : MonoBehaviour
{
    [SerializeField]
    AudioSource strikeSound;
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
            if (strikeSound != null)
            {
                strikeSound.Play();
            }
        }
    }
}
