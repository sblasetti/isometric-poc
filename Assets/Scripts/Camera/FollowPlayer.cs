using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform PlayerTransform;
    [Range(0.01f, 10.0f)]
    public float CameraSpeed = 1.5f;

    private Vector3 cameraOffset;

    void Start()
    {
        cameraOffset = transform.position - PlayerTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var newPosition = PlayerTransform.position + cameraOffset;

        var cameraMoveDirection = (newPosition - transform.position).normalized;
        var distance = Vector3.Distance(newPosition, transform.position);

        if (distance > 0)
        {
            var newCameraPosition = transform.position + cameraMoveDirection * distance * CameraSpeed * Time.deltaTime;

            var distanceAfterMoving = Vector3.Distance(newCameraPosition, newPosition);
            if (distanceAfterMoving > distance)
            {
                // Overshoot the target
                newCameraPosition = newPosition;
            }

            transform.position = newCameraPosition;
        }
    }
}
