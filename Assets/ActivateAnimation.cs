using UnityEngine;

public class ActivateAnimation : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    [SerializeField]
    float delay;
    [SerializeField]
    GameObject target;
    
    private float timer;
    private bool activated;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        activated = true;
        timer = delay;
    }

    // Update is called once per frame
    void Update()
    {
        if (!activated)
            return;
        
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            activated = false;

            // Get device gravity vector (phone orientation)
            Vector3 gravity = Input.acceleration.normalized;
        
            // Calculate phone tilt rotation
            Quaternion phoneTiltRotation = Quaternion.FromToRotation(Vector3.up, gravity);
        
            // Apply final rotation to the object
            transform.rotation = Quaternion.Slerp(transform.rotation, phoneTiltRotation, Time.deltaTime * 5f);
            GameObject newTarget = Instantiate(target, transform.position, transform.rotation);
            newTarget.transform.SetParent(transform, true); 
        }
        
    }
}
