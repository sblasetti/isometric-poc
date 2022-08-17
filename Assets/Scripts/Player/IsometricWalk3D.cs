using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricWalk3D : MonoBehaviour
{
    public CharacterController characterController;
    public Transform cameraTransform;
    public float speed = 6f;
    public bool lookAtPointer = false;
    public KeyCode lookAtPointerKey = KeyCode.LeftShift;

    public float turnSmoothTime = 0.05f;
    float turnSmoothVelocity;

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // Character rotation considering the camera
            float targetDirectionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float directionAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetDirectionAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = GetPlayerRotation(directionAngle);

            // Take into consideration the camera's position
            Vector3 movementDirection = Quaternion.Euler(0f, targetDirectionAngle, 0f) * Vector3.forward;

            // Character movement
            characterController.Move(movementDirection.normalized * speed * Time.deltaTime);
        }
    }

    private Quaternion GetPlayerRotation(float directionAngle)
    {
        if (lookAtPointer && Input.GetKey(lookAtPointerKey))
        {
            var playerPlane = new Plane(Vector3.up, transform.position);
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitDistance;
            if (playerPlane.Raycast(ray, out hitDistance))
            {
                var targetPoint = ray.GetPoint(hitDistance);
                var targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                return Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
            }
        }

        return Quaternion.Euler(0f, directionAngle, 0f);
    }
}
