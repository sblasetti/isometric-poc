using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricZoom : MonoBehaviour
{
    [Range(0.01f, 10.0f)]
    public float ZoomFactor = 8f;
    [Range(10.1f, 20.0f)]
    public float MaxZoom = 15f;
    [Range(0.01f, 10.0f)]
    public float MinZoom = 5f;

    private Camera activeCamera;

    // Start is called before the first frame update
    void Start()
    {
        activeCamera = transform.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        var zoomDifference = GetZoomDifference();
        
        if (zoomDifference != 0)
        {
            var newCameraZoomSize = activeCamera.orthographicSize - zoomDifference;
            Camera.main.orthographicSize = Mathf.Clamp(newCameraZoomSize, MinZoom, MaxZoom);
        }
    }

    private float GetZoomDifference()
    {
        if (Input.GetKey(KeyCode.Plus) || Input.GetKey(KeyCode.KeypadPlus))
        {
            return ZoomFactor * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus))
        {
            return -ZoomFactor * Time.deltaTime;
        }

        // TODO: Have different factors depending on the key used to change the zoom
        return Input.mouseScrollDelta.y * ZoomFactor * Time.deltaTime;
    }
}
