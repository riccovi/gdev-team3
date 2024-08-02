using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PlayStats playerStats;
    // Start is called before the first frame update
    private void Awake() {
        instance=this;
        playerStats =GameObject.FindObjectOfType<PlayStats>();
    }
    void Start()
    {
        UIHandler.instance.setHealthAtStart(playerStats.MaxHealth);
    }

    public void AddUIItem(Sprite Icon)
    {
        Debug.Log("Add Item");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {
        Debug.Log("GameOver");
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
