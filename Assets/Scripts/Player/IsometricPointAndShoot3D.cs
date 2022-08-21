using UnityEngine;

public class IsometricPointAndShoot3D : MonoBehaviour
{
    public float turnSpeed = 6f;
    public KeyCode lookAtPointerKey = KeyCode.Mouse1;
    public float turnSmoothTime = 0.05f;

    private bool isPointing;

    void Update()
    {
        isPointing = Input.GetKey(lookAtPointerKey);

        if (isPointing)
        {
            // Character rotation considering the camera
            var playerPlane = new Plane(Vector3.up, transform.position);
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitDistance;
            if (playerPlane.Raycast(ray, out hitDistance))
            {
                var targetPoint = ray.GetPoint(hitDistance);
                var targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            }
        }
    }

    public bool IsPointing()
    {
        return isPointing;
    }
}
