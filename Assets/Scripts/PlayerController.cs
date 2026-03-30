using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public Transform cameraPivot;

    [Header("Interaction")]
    public float grabDistance = 3f;
    public Transform holdPoint;
    public float force = 10;
    public float throwArc = 0.3f;

    [Header("Jump")]
    public float jumpForce = 6f;
    public LayerMask groundLayer;

    bool isGrounded;
    Rigidbody rb;

    Rigidbody heldBall;
    Camera cam;

    float xRotation;
    Vector3 lastMousePos;
    Vector3 mouseVelocity;

    void Start()
    {
        cam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
        Look();
        HandleGrab();
        HandleThrow();
        TrackMouseVelocity();

        CheckGround();
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = transform.right * h + transform.forward * v;
        Vector3 targetVelocity = move * moveSpeed;
        rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
    }

    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -70f, 70f);

        cameraPivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleGrab()
    {
        if (Input.GetMouseButtonDown(0) && heldBall == null)
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            if (Physics.Raycast(ray, out RaycastHit hit, grabDistance))
            {
                if (hit.rigidbody != null && hit.rigidbody.CompareTag("Ball"))
                {
                    heldBall = hit.rigidbody;
                    heldBall.useGravity = false;
                    heldBall.linearVelocity = Vector3.zero;
                }
            }
        }

        if (heldBall != null)
        {
            heldBall.position = Vector3.Lerp(
                heldBall.position,
                holdPoint.position,
                Time.deltaTime * 15f
            );
        }
    }

    void HandleThrow()
    {
        if (Input.GetMouseButtonUp(0) && heldBall != null)
        {
            heldBall.useGravity = true;

            Vector3 dir = cam.transform.forward;
            dir += Vector3.up * throwArc;
            dir.Normalize();

            heldBall.linearVelocity = dir * force;
            heldBall.AddTorque(Random.insideUnitSphere * 10f, ForceMode.Impulse);
            heldBall = null;
        }
    }

    void TrackMouseVelocity()
    {
        Vector3 current = Input.mousePosition;
        mouseVelocity = (current - lastMousePos) / Time.deltaTime;
        lastMousePos = current;
    }

    void CheckGround()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);
    }
}