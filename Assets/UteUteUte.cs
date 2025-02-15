using System.Collections;
using Lean.Touch;
using UnityEngine;

public class UteUteUte : MonoBehaviour
{
    [SerializeField] private GameObject myBallsPrefab;
    [SerializeField] private AudioSource cannonSound;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LeanTouch.OnFingerTap += LeanTouchOnOnFingerTap;        
    }

    private void LeanTouchOnOnFingerTap(LeanFinger obj)
    {
        Debug.Log("FIRE ON FINGER TAP");
        Ray ray = Camera.main.ScreenPointToRay(obj.ScreenPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Weapon"))
            {
                Debug.Log("FIRE UTE! UTE! UTE!");
                GameObject ball = Instantiate(myBallsPrefab, 
                                            transform.position, 
                                            transform.rotation);
                Rigidbody rigidBody = ball.GetComponent<Rigidbody>();
                rigidBody.AddForce(transform.forward * 0.75f, ForceMode.Impulse);
                if (cannonSound != null)
                {
                    cannonSound.pitch = Random.Range(0.8f, 1.2f);
                    cannonSound.Play();
                }
                StartCoroutine(DestroyBall(ball, 1.0f));
            }
        }
    }

    private IEnumerator DestroyBall(GameObject ball, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(ball);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
