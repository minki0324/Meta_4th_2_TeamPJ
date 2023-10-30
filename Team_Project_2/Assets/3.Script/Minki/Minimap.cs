using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    [SerializeField] private Camera minimap_Camera;
    [SerializeField] private float zoomMin = 1f;
    [SerializeField] private float zoomMax = 30f;
    [SerializeField] private float zoomOneStep = 1f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            Zoom(KeyCode.KeypadPlus);
        }
        else if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            Zoom(KeyCode.KeypadMinus);
        }
    }

    private void Zoom(KeyCode keyCode)
    {
        switch(keyCode)
        {
            case KeyCode.KeypadPlus:
                minimap_Camera.orthographicSize = Mathf.Max(minimap_Camera.orthographicSize - zoomOneStep, zoomMin);
                break;
            case KeyCode.KeypadMinus:
                minimap_Camera.orthographicSize = Mathf.Min(minimap_Camera.orthographicSize + zoomOneStep, zoomMax);
                break;

        }
    }
}
