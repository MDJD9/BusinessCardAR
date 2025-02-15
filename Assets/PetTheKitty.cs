using System;
using Lean.Touch;
using UnityEngine;
using Random = UnityEngine.Random;

public class PetTheKitty : MonoBehaviour
{
    [SerializeField]
    private AudioSource purr;
    [SerializeField]
    private AudioSource meow;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LeanTouch.OnFingerSwipe += PetTheKittyOnSwipe;
        LeanTouch.OnFingerTap += BoopTheKitty;
    }

    private void BoopTheKitty(LeanFinger finger)
    {
        Ray ray = Camera.main.ScreenPointToRay(finger.ScreenPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Pet"))
            {
                if (purr != null)
                {
                    purr.pitch = Random.Range(0.9f, 1.1f);
                    purr.Play();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        Lean.Touch.LeanTouch.OnFingerSwipe -= PetTheKittyOnSwipe;
        Lean.Touch.LeanTouch.OnFingerDown -= BoopTheKitty;
    }

    public void PetTheKittyOnSwipe(LeanFinger finger)
    {
        // // Ray ray = Camera.main.ScreenPointToRay(finger.ScreenPosition);
        // // RaycastHit hit;
        // // if (Physics.Raycast(ray, out hit))
        // // {
        // //     if (hit.collider.CompareTag("Pet"))
        // //     {
        //         Camera theCamera = Camera.main;
        //         StressReceiver sr = theCamera.GetComponent<StressReceiver>();
        //         sr.InduceStress(3.0f);
        //         Debug.Log("The kitty has been pet");
        //         purr.pitch = Random.Range(0.5f, 1.5f);
        //         purr.Play();
        //     // }
        // // }
    }
}
