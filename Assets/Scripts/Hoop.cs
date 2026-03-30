using UnityEngine;

public class Hoop : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ball")) return;
        
        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;
        
        bool isBallMovingDown = rb.linearVelocity.y < 0f && rb.position.y > transform.position.y;
        if (!isBallMovingDown) return;

        if (other.CompareTag("Ball"))
        {
            Ball ball = other.GetComponent<Ball>();

            if (ball != null)
            {
                if (ball.IsCleanShot())
                    GameManager.Instance.AddScore(2);
                else
                    GameManager.Instance.AddScore(1);

                ball.ResetShot();
            }
        }
    }
}