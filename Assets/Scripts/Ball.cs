using UnityEngine;

public class Ball : MonoBehaviour
{
    Vector3 originalPosition;
    Quaternion originalRotation;

    Rigidbody rb;
    bool touchedRim = false;

    void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        rb = GetComponent<Rigidbody>();
        rb.mass = 0.6f;
        rb.linearDamping = 0.3f;
        rb.angularDamping = 0.05f;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Rim"))
        {
            touchedRim = true;

            rb.linearVelocity *= 0.8f;
            rb.AddTorque(Random.insideUnitSphere * 5f, ForceMode.Impulse);
        }
    }

    public bool IsCleanShot()
    {
        return !touchedRim;
    }

    public void ResetShot()
    {
        touchedRim = false;
    }

    public void ResetBall()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.position = originalPosition;
        transform.rotation = originalRotation;

        touchedRim = false;
    }
}