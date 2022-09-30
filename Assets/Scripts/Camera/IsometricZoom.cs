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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var newZoomSize = Camera.main.orthographicSize - Input.GetAxis("Mouse ScrollWheel") * ZoomFactor;
        Camera.main.orthographicSize = Mathf.Min(Mathf.Max(newZoomSize, MinZoom), MaxZoom);
    }
}
