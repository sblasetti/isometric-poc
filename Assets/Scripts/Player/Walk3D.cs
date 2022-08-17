using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk3D : MonoBehaviour
{
    public CharacterController controller;
    public Transform camera;
    public float speed = 6f;

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
            float targetRotationAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            float rotationAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotationAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, rotationAngle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetRotationAngle, 0f) * Vector3.forward;

            // Character movement
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }
}
