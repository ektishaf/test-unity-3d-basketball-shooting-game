using UnityEngine;

public class BallTrailController : MonoBehaviour
{
    private TrailRenderer trail;
    private Rigidbody rb;

    public float velocityThreshold = 0.5f;

    void Start()
    {
        trail = GetComponent<TrailRenderer>();
        rb = GetComponent<Rigidbody>();
        trail.emitting = false;
        trail.time = 0.3f;
        trail.startWidth = 0.1f;
        trail.endWidth = 0f;
    }

    void Update()
    {
        if (rb.linearVelocity.magnitude > velocityThreshold)
        {
            if (!trail.emitting)
                trail.emitting = true;
        }
        else
        {
            if (trail.emitting)
                trail.emitting = false;
        }
    }

    public void OnThrow()
    {
        trail.Clear();
        trail.emitting = true;
    }

    public void OnReset()
    {
        trail.emitting = false;
        trail.Clear();
    }
}