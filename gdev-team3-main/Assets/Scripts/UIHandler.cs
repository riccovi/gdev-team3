using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
     [Header("Heatlh")]
    // Start is called before the first frame update
    public static UIHandler instance;

    public Transform healhParent;

    public GameObject healthPrefab;

    [Header("Ivnentory")]
    public Transform InventoryParent;
    public GameObject InvPrefab;

    [Header("Pause")]
    public GameObject PauseMenu;

    [Header("Game Over")]
    public GameObject GameOverWindow;

    [Header("Winning Screen")]
    public GameObject WinWindow;

    [Header("Dialogue")]
    public GameObject DialogueWindow;

    public Transform Background;
    public TextMeshProUGUI textDialogue;

    public float ScaleTime=2.0f;

    public float TextApearTime=3.0f;

    public float AfterTextWait=3f;

    private float WaitTime=5.0f;

    private Vector3 BackgroundOrigScale;

    public AudioClip popUpSound;



    void Awake() {
        instance=this;
        
    }
    void Start()
    {
        WaitTime=TextApearTime+AfterTextWait;
        //setup UI health
        Background=DialogueWindow.transform.Find("Background");
        textDialogue=DialogueWindow.transform.Find("Text").GetComponent<TextMeshProUGUI>();

        BackgroundOrigScale = Background.localScale;

        setHealthAtStart(GameManager.instance.playerStats.MaxHealth);
        setInventory(GameManager.instance.PlayerMove.GetComponent<InventoryManager>().InventorySize);
    }

    public void setInventory(int Size)
    {
        for(int i=0;i<Size;i++)
        {
            var obj = Instantiate(InvPrefab,InventoryParent);
            obj.gameObject.SetActive(false);
            obj.name="0x0";
        }
    }

    public void ActivatePopUp()
    {
        DialogueWindow.gameObject.SetActive(true);
        StartCoroutine(ScaleOverTime(ScaleTime,"0000",WaitTime));
    }

    public void ActivatePopUp(string text)
    {
        StopAllCoroutines();
        textDialogue.text="";

        DialogueWindow.gameObject.SetActive(true);
        StartCoroutine(ScaleOverTime(ScaleTime,text,WaitTime));
    }

    IEnumerator ScaleOverTime(float duration,string Text,float WaitTime)
    {
        Debug.Log("Scale");
        // Get the current scale (should be Vector3.zero initially)
        Vector3 initialScale = Vector3.zero;

        // Calculate the amount of time passed
        float timePassed = 0f;

        // Loop until the timePassed is greater than duration
        while (timePassed < duration)
        {
            // Increment timePassed by the time passed since the last frame
            timePassed += Time.deltaTime;

            // Calculate the progress (0 to 1)
            float progress = Mathf.Clamp01(timePassed / duration);

            // Lerp between the initial scale and the original scale based on progress
            Background.localScale = Vector3.Lerp(initialScale, BackgroundOrigScale, progress);

            // Yield the coroutine until the next frame
            yield return null;
        }

        // Ensure the final scale is exactly the original scale
        Background.localScale = BackgroundOrigScale;

        textDialogue.gameObject.SetActive(true);

        StartCoroutine(TypeText(textDialogue,Text));

        yield return new WaitForSeconds(WaitTime);

        textDialogue.gameObject.SetActive(false);

        DialogueWindow.gameObject.SetActive(false);


    }

    IEnumerator TypeText(TMP_Text textComponent,string fullText)
    {
        textComponent.text = ""; // Clear the initial text
        float timePerCharacter = TextApearTime / fullText.Length; // Calculate time per character

        foreach (char letter in fullText.ToCharArray())
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(timePerCharacter);
        }
    }


    public void addInventory(string ID,Sprite ico)
    {
        foreach (Transform child in InventoryParent)
        {
            if (!child.gameObject.activeInHierarchy)
            {
                child.gameObject.SetActive(true);
                child.name=ID;
                child.GetComponent<Image>().sprite=ico;
                Debug.Log("Activated child: " + child.gameObject.name);
                return;
            }
        }
        Debug.Log("No inactive child found.");
    }

    public void removeInventory(string ID)
    {
        foreach (Transform child in InventoryParent)
        {
            if (child.name==ID)
            {
                child.gameObject.SetActive(false);
                child.name="0x0";
            }
        }
        Debug.Log("No inactive child found.");
    }

    public IEnumerator GameOver(bool winscreen)
    {
        yield return new WaitForSeconds(1);
        GameManager.instance.currentState=GameManager.gameStatus.Pause;

        if(winscreen)
        {
            Sound_Manager.instance.environmentSoundOnce("CompleteLevel");
            WinWindow.gameObject.SetActive(true);
        }
        else
        {
            Sound_Manager.instance.environmentSoundOnce("Deathscreen");
            GameOverWindow.gameObject.SetActive(true);
        }
        
        
        

        yield return new WaitForSeconds(3);

        if(winscreen)
        {
            GameManager.instance.EndLevel();
        }
        else
        {
            GameManager.instance.GameOver();
        }

        
    }

    
    public void setHealthAtStart(int MaxHealth)
    {
        for(int i=0;i<MaxHealth;i++)
        {
            Instantiate(healthPrefab,healhParent);
        }
    }

    public void removeHealth()
    {
        DeactivateLastActiveChild(healhParent);
    }

    public void addHealth()
    {
        ActivateFirstInactiveChild(healhParent);
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateFirstInactiveChild(Transform healthParent)
    {
        foreach (Transform child in healthParent)
        {
            if (!child.gameObject.activeInHierarchy)
            {
                child.gameObject.SetActive(true);
                Debug.Log("Activated child: " + child.gameObject.name);
                return;
            }
        }

        Debug.Log("No inactive child found.");
    }

    // Method to find the last active child and deactivate it
    public void DeactivateLastActiveChild(Transform healthParent)
    {
        for (int i = healthParent.childCount - 1; i >= 0; i--)
        {
            Transform child = healthParent.GetChild(i);
            if (child.gameObject.activeInHierarchy)
            {
                child.gameObject.SetActive(false);
                Debug.Log("Deactivated child: " + child.gameObject.name);
                return;
            }
        }

        Debug.Log("No active child found.");
    }
}
