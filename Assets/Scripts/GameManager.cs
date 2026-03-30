using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score;
    public Text scoreText;
    public Camera cam;

    Vector3 camOriginalPos;

    void Awake()
    {
        Instance = this;
        camOriginalPos = cam.transform.localPosition;
    }

    void Start()
    {
        Application.targetFrameRate = 120;
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = "Score: " + score;

        StartCoroutine(Feedback());
    }

    IEnumerator Feedback()
    {
        Time.timeScale = 0.5f;

        float t = 0;
        while (t < 0.15f)
        {
            cam.transform.localPosition = camOriginalPos + Random.insideUnitSphere * 0.1f;
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        cam.transform.localPosition = camOriginalPos;

        yield return new WaitForSecondsRealtime(0.1f);

        Time.timeScale = 1f;
    }
}