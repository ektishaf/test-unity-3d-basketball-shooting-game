using UnityEngine;
using UnityEngine.InputSystem;

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
    Vector2 moveInput;
    Vector2 lookInput;

    void Start()
    {
        cam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            GameManager.Instance.ToggleSettings();
        }

        if(Cursor.lockState == CursorLockMode.None) return;

        ReadInputs();
        Move();
        Look();
        HandleGrab();
        CheckGround();
    }

    void ReadInputs()
    {
        moveInput = Vector2.zero;
        if (Keyboard.current.wKey.isPressed) moveInput.y = 1;
        if (Keyboard.current.sKey.isPressed) moveInput.y = -1;
        if (Keyboard.current.aKey.isPressed) moveInput.x = -1;
        if (Keyboard.current.dKey.isPressed) moveInput.x = 1;

        lookInput = Mouse.current.delta.ReadValue();

        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void Move()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        Vector3 targetVelocity = move * moveSpeed;
        rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
    }

    void Look()
    {
        float mouseX = lookInput.x * mouseSensitivity * 0.1f;
        float mouseY = lookInput.y * mouseSensitivity * 0.1f;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -70f, 70f);

        cameraPivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleGrab()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && heldBall == null)
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
            heldBall.position = Vector3.Lerp(heldBall.position, holdPoint.position, Time.deltaTime * 15f);

            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                HandleThrow();
            }
        }
    }

    void HandleThrow()
    {
        heldBall.useGravity = true;

        Vector3 dir = cam.transform.forward;
        dir += Vector3.up * throwArc;
        dir.Normalize();

        heldBall.linearVelocity = dir * force;
        heldBall.AddTorque(Random.insideUnitSphere * 10f, ForceMode.Impulse);
        heldBall = null;
    }

    void CheckGround()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);
    }
}