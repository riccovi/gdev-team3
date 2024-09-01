using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PlayStats playerStats;

    public LevelManager LevelManager;

    public playerMovement PlayerMove;

    public bool LoadingLevel;
    // Start is called before the first frame update
    private void Awake() {
        instance=this;
        playerStats =GameObject.FindObjectOfType<PlayStats>();
    }
    void Start()
    {
        LoadingLevel=false;
        PlayerMove=FindObjectOfType<playerMovement>();
        UIHandler.instance.setHealthAtStart(playerStats.MaxHealth);
    }

    public void AddUIItem(Sprite Icon)
    {
        Debug.Log("Add Item");
    }

    // Update is called once per frame

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    
    public void UpgradeDoubleJump()
    {
        PlayerMove.ExtraJumps=1;
    }
    
    public void EndLevel()
    {
        Debug.Log("End Level");
    }

    public void GameOver()
    {
        if(!LoadingLevel)
        {
            LoadingLevel=true;
            Debug.Log("GameOver");
            LevelManager.RestartLevel();
        }

    }

    public string ExtractNumbers(string input)
    {
        // Find the index of the underscore
        int underscoreIndex = input.IndexOf('_');
        
        // If an underscore is found, extract the substring after it
        if (underscoreIndex != -1 && underscoreIndex + 1 < input.Length)
        {
            return input.Substring(underscoreIndex + 1);
        }

        // Return an empty string if no underscore is found or the underscore is at the end
        return string.Empty;
    }

}
