using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public Slider volumeSlider;
    public TextMeshProUGUI volumeLabel;

    void Start()
    {
        volumeSlider.value = AudioListener.volume;
        volumeLabel.text = "Volume: " + (volumeSlider.value * 100).ToString("0");

        volumeSlider.onValueChanged.AddListener(delegate { OnVolumeChange(); });
    }

    public void OnVolumeChange()
    {
        AudioListener.volume = volumeSlider.value;
        volumeLabel.text = "Volume: " + (volumeSlider.value * 100).ToString("0");
    }
}
