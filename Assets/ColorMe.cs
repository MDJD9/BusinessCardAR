using System;
using Lean.Touch;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ColorMe : MonoBehaviour
{
    private TMP_Text tmpText;

    void Start()
    {
        tmpText = GetComponent<TMP_Text>();
        LeanTouch.OnFingerTap += LeanTouchOnOnFingerTap;
    }

    private void OnDestroy()
    {
        LeanTouch.OnFingerTap -= LeanTouchOnOnFingerTap;
    }

    private void LeanTouchOnOnFingerTap(LeanFinger obj)
    {
        Ray ray = Camera.main.ScreenPointToRay(obj.ScreenPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "ColorMe")
            {
                ChangeColor();
            }
        }
    }

    void ChangeColor()
    {
        if (tmpText == null)
            return;
        
        tmpText.fontMaterial.SetColor(ShaderUtilities.ID_FaceColor, new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
        tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
