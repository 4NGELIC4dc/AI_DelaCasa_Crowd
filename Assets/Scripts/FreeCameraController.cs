using UnityEngine;

public class FreeCameraController : MonoBehaviour
{
    public Transform target;            
    public float distance = 10.0f;       
    public float zoomSpeed = 5.0f;      
    public float rotationSpeed = 3.0f;  
    public float panSpeed = 0.5f;      

    private float currentYaw = 0.0f;
    private float currentPitch = 20.0f;

    private Vector3 lastMousePosition;

    void Update()
    {
        // Zoom in and out (roll scroll wheel)
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance -= scroll * zoomSpeed;
        distance = Mathf.Clamp(distance, 5f, 50f);

        // Rotate around (hold right button)
        if (Input.GetMouseButton(1))
        {
            currentYaw += Input.GetAxis("Mouse X") * rotationSpeed;
            currentPitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
            currentPitch = Mathf.Clamp(currentPitch, 10f, 80f);
        }

        // Pan/drag camera (hold scroll wheel)
        if (Input.GetMouseButtonDown(2))
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(2))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            Vector3 right = transform.right;
            Vector3 up = transform.up;

            Vector3 pan = (-right * delta.x - up * delta.y) * panSpeed * Time.deltaTime;
            target.position += pan;
            lastMousePosition = Input.mousePosition;
        }
    }

    void LateUpdate()
    {
        if (!target) return;

        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        Vector3 dir = new Vector3(0, 0, -distance);
        transform.position = target.position + rotation * dir;
        transform.LookAt(target);
    }
}
