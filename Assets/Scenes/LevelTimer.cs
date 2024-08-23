using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelTimer : MonoBehaviour
{
    public float timeRemaining = 120f; // 2 minutes
    public bool timerIsRunning = false;
    public Text timerText;

    // Start is called before the first frame update
    void Start()
    {
        timerIsRunning = true;
        UpdateTimerDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        if(timerIsRunning)
        {
            if(timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay();
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                UpdateTimerDisplay();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining/60);
        int seconds = Mathf.FloorToInt(timeRemaining%60);
        timerText.text = string.Format("{0:00}:{1:00}",minutes,seconds);

        if(timeRemaining <= 10)
        {
            timerText.color = Color.red;
        }
        else
        {
            timerText.color = Color.black;
        }
    }
}
