using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.Video;

public class Interactions : MonoBehaviour
{

    [FormerlySerializedAs("_interactionList")] [SerializeField] 
    private List<VideoClip> interactionList;
    private VideoPlayer[] _videoPlayers;
    private bool _pressed = false;
    private bool _wasTouchingLastFrame = false;
    private Camera _camera;
    private bool toggle = true;
    [SerializeField]
    private AudioSource interactionAudio;


    private int toggleValue()
    {
        return toggle ? 1 : 0;
    }
    
    private void CheckForInteractions()
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
                    foreach (VideoPlayer videoPlayer in _videoPlayers)
                    {
                        if (videoPlayer.clip != interactionList[toggleValue()]) // Only change if it's a different clip
                        {
                            Debug.Log("ARProjecto - Toggling clip");
                            videoPlayer.clip = interactionList[toggleValue()];
                            videoPlayer.Stop();
                            videoPlayer.Play();
                            toggle = !toggle;
                            if (interactionAudio != null)
                            {
                                interactionAudio.pitch = Random.Range(0.8f, 1.2f);
                                interactionAudio.Play();
                            }
                        }
                        
                    }
                }
            }
        }
        _wasTouchingLastFrame = isTouching;
    }    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetVideoPlayers();
        _camera = Camera.main;
    }

    private void Awake()
    {
        GetVideoPlayers();
        _camera = Camera.main;
    }

    void GetVideoPlayers()
    {
        _videoPlayers = gameObject.GetComponentsInChildren<VideoPlayer>();
        if (_videoPlayers.Length == 0)
        {
            Debug.LogError("ARProjecto - No video player");
        }
        else
        {
            Debug.Log("ARProjecto - " + _videoPlayers.Length + "Video player found");
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        CheckForInteractions();
    }
}
