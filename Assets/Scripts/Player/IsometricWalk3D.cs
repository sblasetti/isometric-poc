using UnityEngine;

public class IsometricWalk3D : MonoBehaviour
{
    public CharacterController characterController;
    public Transform cameraTransform;
    public float regularSpeed = 6f;
    public float slowSpeed = 3f;

    public float turnSmoothTime = 0.05f;
    float turnSmoothVelocity;
    IsometricPointAndShoot3D pointAndShootBehavior;

    void Start()
    {
        pointAndShootBehavior = transform.GetComponent<IsometricPointAndShoot3D>();
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            var isJustWalking = pointAndShootBehavior == null || !pointAndShootBehavior.IsPointing();

            // Character rotation considering the camera
            float targetDirectionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;

            // Only rotate the character based on movement if not pointing
            if (isJustWalking)
            {
                float directionAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetDirectionAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, directionAngle, 0f);
            }

            // Take into consideration the camera's position
            Vector3 movementDirection = Quaternion.Euler(0f, targetDirectionAngle, 0f) * Vector3.forward;

            // Character movement
            var movementSpeed = isJustWalking ? regularSpeed : slowSpeed;
            characterController.Move(movementDirection.normalized * movementSpeed * Time.deltaTime);
        }
    }
}
