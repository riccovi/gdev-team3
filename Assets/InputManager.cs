using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameManager.instance.currentState==GameManager.gameStatus.Run)
            {
                GameManager.instance.PauseMenu();
            }
            else
            {
                GameManager.instance.UnPauseMenu();
            }
        }
    }
}
