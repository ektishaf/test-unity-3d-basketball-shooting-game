using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public AudioSource audioSource;
    public AudioClip swishAudioClip;
    public AudioClip cheerAudioClip;

    public GameObject settingsPanel;
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

    void SetSettingsVisibility(bool visible)
    {
        settingsPanel.SetActive(visible ? true : false);
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void ToggleSettings()
    {
        SetSettingsVisibility(!settingsPanel.activeSelf);
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

    public void PlaySwishAudio()
    {
        audioSource.clip = swishAudioClip;
        audioSource.Play();
    }

    public void PlayCheerAudio()
    {
        audioSource.clip = cheerAudioClip;
        audioSource.Play();
    }
}