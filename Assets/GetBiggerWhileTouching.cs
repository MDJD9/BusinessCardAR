using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class GetBiggerWhileTouching : MonoBehaviour
{
    [SerializeField] private float rate = 0.05f;
    [SerializeField] private float maximumSize = 0.085f;
    [SerializeField] private float minimumSize = 0.03f;
    
    private bool _wasTouchingLastFrame;
    private Camera _camera;
    private bool collision = false;
    private bool stop = true;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _wasTouchingLastFrame = false;
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        CheckCollision();
        if (stop)
            return;
        
        if (!_wasTouchingLastFrame)
        {
            collision = false;
        }
        
        var scale = gameObject.transform.localScale;
        float val = collision ? rate : -rate;
        scale += gameObject.transform.up * val * Time.deltaTime;
        scale = ClipScale(scale);

        gameObject.transform.localScale = new Vector3(scale.x, scale.y, scale.z);
    }

    private Vector3 ClipScale(Vector3 scale)
    {
        scale.y = ScaleComponent(scale.y);
        scale.x = ScaleComponent(scale.x);
        scale.z = ScaleComponent(scale.z);
        return new Vector3(scale.x, scale.y, scale.z);
    }

    private float ScaleComponent(float scale)
    {
        if (scale < minimumSize && !collision)
        {
            scale = minimumSize;
            stop = true;
        }
        if (scale > maximumSize && !collision)
        {
            scale = maximumSize;
            stop = true;
        }
        return scale;
    }
    
    void CheckCollision()
    {
        if (Touchscreen.current == null) return;  // Safety check for null reference

        bool isTouching = Touchscreen.current.primaryTouch.press.ReadValue() >= 1.0f;

        if (isTouching && !_wasTouchingLastFrame) // Detects the first frame of touch
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            Ray ray = _camera.ScreenPointToRay(touchPosition);
            
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    collision = true;
                }
            }

            stop = false;
        }
        _wasTouchingLastFrame = isTouching;        
    }
}
