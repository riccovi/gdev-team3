using System.Collections;
using System.Collections.Generic;
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

    private void Awake() {
        instance=this;
    }
    void Start()
    {
        //setup UI health
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
