using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 3, -6);

    public float mouseSensitivity = 200f;
    public float touchSensitivity = 0.2f;
    public float smoothSpeed = 10f;

    float xRotation = 0f;
    float yRotation = 0f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        xRotation = angles.x;
        yRotation = angles.y;

        Cursor.lockState = CursorLockMode.Locked; // Solo PC
    }

    void Update()
    {
        RotateCamera();
    }

    void LateUpdate()
    {
        FollowTarget();
    }

    void RotateCamera()
    {
        float mouseX = 0f;
        float mouseY = 0f;

        if (Input.touchCount == 0)
        {
            mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        }
        else
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                mouseX = touch.deltaPosition.x * touchSensitivity;
                mouseY = touch.deltaPosition.y * touchSensitivity;
            }
        }

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -35f, 60f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    void FollowTarget()
    {
        Vector3 desiredPosition = target.position + transform.rotation * offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;
    }
}
