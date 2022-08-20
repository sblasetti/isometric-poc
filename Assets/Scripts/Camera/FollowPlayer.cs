using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform PlayerTransform;
    [Range(0.01f, 1.0f)]
    public float SmoothFactor = 0.5f;

    private Vector3 cameraOffset;

    void Start()
    {
        cameraOffset = transform.position - PlayerTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var newPosition = PlayerTransform.position + cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPosition, SmoothFactor);
    }
}
