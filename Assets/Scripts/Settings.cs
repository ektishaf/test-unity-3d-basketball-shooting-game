using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Slider arcSlider;
    public Slider forceSlider;

    public Text arcValue;
    public Text forceValue;

    public PlayerController playerController;

    void Start()
    {
        arcSlider.value = playerController.throwArc;
        forceSlider.value = playerController.force;

        arcValue.text = playerController.throwArc.ToString();
        forceValue.text = playerController.force.ToString();

        arcSlider.onValueChanged.AddListener((float value) =>
        {
            playerController.throwArc = value;
            arcValue.text = value.ToString();
        });

        forceSlider.onValueChanged.AddListener((float value) =>
        {
            playerController.force = value;
            forceValue.text = value.ToString();
        });
    }
}
