using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    // References for the Settings Panel and Main Menu Panel
    public GameObject settingsPanel;
    public GameObject mainMenuPanel; // Main menu containing Play, Settings, Credits, and Quit buttons
    public Light directionalLight;   // Reference to the directional light for brightness control
    public AudioSource menuMusic;
    public Slider brightnessSlider;
    public Slider volumeSlider;      // Reference to volume slider
    public GameObject creditsPanel;  // Reference to the Credits Panel

    public LevelManager levelManager;

    // Apply saved brightness when the scene starts
    void Start()
    {
        // Load saved brightness and apply it
        if (PlayerPrefs.HasKey("Brightness"))
        {
            float savedBrightness = PlayerPrefs.GetFloat("Brightness");

            // Apply saved brightness to the light
            if (directionalLight != null)
            {
                directionalLight.intensity = savedBrightness;
            }

            // Set the brightness slider value to match the saved brightness
            if (brightnessSlider != null)
            {
                brightnessSlider.value = savedBrightness;
            }
        }

        // Load saved volume and apply it.
        if (PlayerPrefs.HasKey("Volume"))
        {
            float savedVolume = PlayerPrefs.GetFloat("Volume");
            if (menuMusic != null)
            {
                menuMusic.volume = savedVolume;
            }
            if (volumeSlider != null)
            {
                volumeSlider.value = savedVolume;
            }
        }
        
        // Start the music if it's not playing
        if (menuMusic != null && !menuMusic.isPlaying)
        {
            menuMusic.Play();
        }
    }

    // Play button
    public void PlayGame()
    {
        LevelManager.BackMainMenu();
    }

    // Settings button
    public void OpenSettings()
    {
        if (settingsPanel != null && mainMenuPanel != null)
        {
            settingsPanel.SetActive(true);
            mainMenuPanel.SetActive(false);
            Debug.Log("Settings Panel opened.");
        }
    }

    // Back button in the Settings Panel
    public void BackToMainMenu()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    // Close Settings Button (inside the Settings Panel)
    public void CloseSettings()
    {
        if (settingsPanel != null && mainMenuPanel != null)
        {
            settingsPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
        }
    }

    // Adjust the overall volume
    public void AdjustVolume(float volume)
    {
        if (menuMusic != null)
        {
            menuMusic.volume = volume;
        }

        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();

        Debug.Log("Volume adjusted to: " + volume);
    }

    // Adjust the brightness of the directional light
    public void AdjustBrightness(float brightness)
    {
        if (directionalLight != null)
        {
            directionalLight.intensity = brightness;
            PlayerPrefs.SetFloat("Brightness", brightness);
            PlayerPrefs.Save();
        }
    }

    // Open Credits panel
    public void OpenCredits()
    {
        if (creditsPanel != null && mainMenuPanel != null)
        {
            creditsPanel.SetActive(true);
            mainMenuPanel.SetActive(false);
        }
    }



    // Close Credits panel
    public void CloseCredits()
    {
        if (creditsPanel != null && mainMenuPanel != null)
        {
            creditsPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
        }
    }

    // Quit button
    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
        Debug.Log("Game is exiting");
    }
}
